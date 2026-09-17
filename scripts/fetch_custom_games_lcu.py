#!/usr/bin/env python3
"""Dump custom games from the local League client (LCU API), and optionally import them.

The public Riot API (match-v5) does NOT expose custom games: they are absent
from /matches/by-puuid/{puuid}/ids, and fetching one by ID returns a stub with
endOfGameResult="Abort_Unexpected", queueId=0, gameCreation=0 and zero
participants. The 10 queue-0 rows already in the GameOn database are exactly
that: empty shells left by the normal import.

The only source that still holds the real data is the League client itself,
which exposes its own match history (customs included) on a local HTTPS port.
The client MUST be running and logged in on an account that played the game.

Usage:
    python fetch_custom_games_lcu.py                  # last 20 games, keep customs
    python fetch_custom_games_lcu.py --count 200      # look further back
    python fetch_custom_games_lcu.py --all            # keep every game, not just customs
    python fetch_custom_games_lcu.py --no-timeline    # skip the timelines
    python fetch_custom_games_lcu.py --out ./dump     # output directory

    # dump, then import into GameOn! (needs a gameon_admin bearer token)
    python fetch_custom_games_lcu.py --push --token "$GAMEON_TOKEN"
    python fetch_custom_games_lcu.py --push --base-url http://localhost:5184 --token ...

    # import an existing dump, client closed (re-import, or someone else's dump)
    python fetch_custom_games_lcu.py --from-dir ./custom-games --token ...

Each kept game is written as <platformId>_<gameId>.json (e.g. EUW1_7773186456.json),
in the LCU format (legacy match-v4 shape). --push posts it to
POST /lol/Match/custom/import, which maps it to the GameOn! model server side.
Aborted games (endOfGameResult other than GameComplete, i.e. practice tool runs and
lobbies nobody joined) are dumped but not pushed, unless --include-aborted.
"""

import argparse
import base64
import json
import os
import platform
import ssl
import sys
import urllib.error
import urllib.request
from pathlib import Path

DEFAULT_BASE_URL = "https://gameon-api.valentinvirot.fr"

LOCKFILE_PATHS = [
    Path("/Applications/League of Legends.app/Contents/LoL/lockfile"),
    Path("C:/Riot Games/League of Legends/lockfile"),
    Path.home() / "Applications/League of Legends.app/Contents/LoL/lockfile",
]

# The client serves its API with a self-signed certificate.
SSL_CONTEXT = ssl.create_default_context()
SSL_CONTEXT.check_hostname = False
SSL_CONTEXT.verify_mode = ssl.CERT_NONE


def find_lockfile(explicit: str | None) -> Path:
    if explicit:
        path = Path(explicit)
        if not path.exists():
            sys.exit(f"Lockfile not found at {path}")
        return path

    for path in LOCKFILE_PATHS:
        if path.exists():
            return path

    sys.exit(
        "Lockfile not found — the League client must be running and logged in.\n"
        f"Looked in:\n  " + "\n  ".join(str(p) for p in LOCKFILE_PATHS) + "\n"
        f"(platform: {platform.system()}). Pass --lockfile if your install is elsewhere."
    )


def read_lockfile(path: Path) -> tuple[str, str]:
    """Returns (base_url, basic_auth_header) read from the client lockfile."""
    # Format: LeagueClient:<pid>:<port>:<password>:https
    parts = path.read_text().strip().split(":")
    if len(parts) < 5:
        sys.exit(f"Unexpected lockfile format in {path}")
    port, password = parts[2], parts[3]
    token = base64.b64encode(f"riot:{password}".encode()).decode()
    return f"https://127.0.0.1:{port}", f"Basic {token}"


def lcu_get(base_url: str, auth: str, route: str):
    req = urllib.request.Request(base_url + route, headers={"Authorization": auth, "Accept": "application/json"})
    try:
        with urllib.request.urlopen(req, timeout=30, context=SSL_CONTEXT) as resp:
            body = resp.read()
            return json.loads(body) if body else None
    except urllib.error.HTTPError as e:
        sys.exit(f"LCU returned HTTP {e.code} on {route}: {e.read()[:300].decode(errors='replace')}")
    except urllib.error.URLError as e:
        sys.exit(f"Cannot reach the League client on {base_url} ({e.reason}). Is it still running?")


def is_custom(game: dict) -> bool:
    return game.get("gameType") == "CUSTOM_GAME" or game.get("queueId") in (0, -1)


def describe(game: dict) -> str:
    identities = {p["participantId"]: p.get("player", {}) for p in game.get("participantIdentities", [])}
    teams: dict[int, list[str]] = {}
    for participant in game.get("participants", []):
        player = identities.get(participant["participantId"], {})
        name = player.get("gameName") or player.get("summonerName") or f"#{participant['participantId']}"
        stats = participant.get("stats", {})
        kda = f"{stats.get('kills', 0)}/{stats.get('deaths', 0)}/{stats.get('assists', 0)}"
        teams.setdefault(participant.get("teamId", 0), []).append(f"{name} ({kda})")

    lines = [
        f"  {game.get('gameCreationDate', '?')} | {game.get('gameMode')} / {game.get('gameType')} "
        f"| queueId={game.get('queueId')} | {game.get('gameDuration', 0) // 60} min"
    ]
    for team_id, players in sorted(teams.items()):
        winner = next(
            (t.get("win") for t in game.get("teams", []) if t.get("teamId") == team_id), "?"
        )
        lines.append(f"    team {team_id} ({winner}): " + ", ".join(players))
    return "\n".join(lines)


def push(base_url: str, token: str, game: dict, timeline: dict | None) -> tuple[bool, str]:
    """Posts one game to the GameOn! API. Returns (succeeded, message)."""
    payload = json.dumps({"game": game, "timeline": timeline}).encode()
    req = urllib.request.Request(
        base_url.rstrip("/") + "/lol/Match/custom/import",
        data=payload,
        method="POST",
        headers={"Content-Type": "application/json", "Authorization": f"Bearer {token}"},
    )

    try:
        with urllib.request.urlopen(req, timeout=120) as resp:
            body = json.loads(resp.read() or b"{}")
    except urllib.error.HTTPError as e:
        return False, f"HTTP {e.code}: {e.read()[:300].decode(errors='replace')}"
    except urllib.error.URLError as e:
        return False, f"cannot reach {base_url} ({e.reason})"

    unlinked = body.get("unlinkedRiotIds") or []
    message = (
        f"{'replaced' if body.get('replaced') else 'imported'}, "
        f"{body.get('participantCount')} participants "
        f"({body.get('linkedPlayerCount')} linked), "
        f"{body.get('timelineFrameCount')} frames, queueId={body.get('queueId')}"
    )
    if unlinked:
        message += " | not linked to any GameOn! player: " + ", ".join(unlinked)
    return True, message


def push_directory(args) -> int:
    """Imports games already dumped on disk, without going through the client."""
    source = Path(args.from_dir)
    files = sorted(f for f in source.glob("*.json") if not f.name.endswith(".timeline.json"))

    if not files:
        sys.exit(f"No dumped game found in {source}")

    pushed, skipped, failed = 0, 0, 0

    for file in files:
        game = json.loads(file.read_text())
        timeline_file = file.with_suffix(".timeline.json")
        timeline = json.loads(timeline_file.read_text()) if timeline_file.exists() else None

        print(f"\n{file.stem}")
        print(describe(game))

        if game.get("endOfGameResult") != "GameComplete" and not args.include_aborted:
            print(f"  not pushed: {game.get('endOfGameResult')} (use --include-aborted to force)")
            skipped += 1
            continue

        ok, message = push(args.base_url, args.token, game, timeline)
        print(f"  {'pushed' if ok else 'PUSH FAILED'}: {message}")
        if ok:
            pushed += 1
        else:
            failed += 1

    print(f"\n{pushed} imported, {skipped} skipped (aborted), {failed} failed.")
    return 1 if failed else 0


def main() -> int:
    parser = argparse.ArgumentParser(description="Dump custom games from the local League client.")
    parser.add_argument("--count", type=int, default=20, help="how many recent games to scan (default: 20)")
    parser.add_argument("--all", action="store_true", help="keep every game, not only custom ones")
    parser.add_argument("--timeline", action="store_true", default=True, help="also dump each game's timeline (default)")
    parser.add_argument("--no-timeline", dest="timeline", action="store_false", help="skip the timelines")
    parser.add_argument("--push", action="store_true", help="import each kept game into GameOn! as well")
    parser.add_argument("--base-url", default=DEFAULT_BASE_URL, help=f"GameOn! API base URL (default: {DEFAULT_BASE_URL})")
    parser.add_argument("--token", default=os.environ.get("GAMEON_TOKEN"), help="gameon_admin bearer token (or GAMEON_TOKEN)")
    parser.add_argument("--include-aborted", action="store_true", help="push aborted games too (practice tool, empty lobbies)")
    parser.add_argument("--from-dir", default=None, help="import games already dumped in this directory instead of reading the client")
    parser.add_argument("--out", default=str(Path(__file__).resolve().parent / "custom-games"), help="output directory")
    parser.add_argument("--lockfile", default=None, help="path to the client lockfile")
    args = parser.parse_args()

    if args.from_dir:
        args.push = True

    if args.push and not args.token:
        sys.exit("Pushing needs a bearer token: pass --token, or set GAMEON_TOKEN.")

    if args.from_dir:
        return push_directory(args)

    base_url, auth = read_lockfile(find_lockfile(args.lockfile))

    summoner = lcu_get(base_url, auth, "/lol-summoner/v1/current-summoner") or {}
    print(f"Connected as {summoner.get('gameName')}#{summoner.get('tagLine')} (level {summoner.get('summonerLevel')}).")

    history = lcu_get(
        base_url, auth,
        f"/lol-match-history/v1/products/lol/current-summoner/matches?begIndex=0&endIndex={args.count - 1}",
    ) or {}
    games = (history.get("games") or {}).get("games") or []
    platform_id = history.get("platformId") or (games[0].get("platformId") if games else "EUW1")
    print(f"{len(games)} games in the client history.")

    kept = games if args.all else [g for g in games if is_custom(g)]
    if not kept:
        print(
            "No custom game in that window. The client only keeps a limited history:\n"
            "  - try --count 200\n"
            "  - or run this on the account of another player of that 5v5."
        )
        return 1

    out_dir = Path(args.out)
    out_dir.mkdir(parents=True, exist_ok=True)

    pushed, skipped, failed = 0, 0, 0

    for game in kept:
        game_id = game["gameId"]
        # The history payload is already complete, but /games/{id} is the authoritative one.
        detail = lcu_get(base_url, auth, f"/lol-match-history/v1/games/{game_id}") or game
        match_id = f"{detail.get('platformId') or platform_id}_{game_id}"

        (out_dir / f"{match_id}.json").write_text(json.dumps(detail, indent=1))
        print(f"\n{match_id} -> {out_dir / f'{match_id}.json'}")
        print(describe(detail))

        timeline = None
        if args.timeline:
            timeline = lcu_get(base_url, auth, f"/lol-match-history/v1/game-timelines/{game_id}")
            if timeline:
                (out_dir / f"{match_id}.timeline.json").write_text(json.dumps(timeline, indent=1))
                print(f"  timeline -> {out_dir / f'{match_id}.timeline.json'} ({len(timeline.get('frames', []))} frames)")

        if not args.push:
            continue

        # Aborted games hold nothing worth importing: a practice tool run, or a lobby that never
        # filled. They still get dumped above, they just don't reach the database.
        if detail.get("endOfGameResult") != "GameComplete" and not args.include_aborted:
            print(f"  not pushed: {detail.get('endOfGameResult')} (use --include-aborted to force)")
            skipped += 1
            continue

        ok, message = push(args.base_url, args.token, detail, timeline)
        print(f"  {'pushed' if ok else 'PUSH FAILED'}: {message}")
        if ok:
            pushed += 1
        else:
            failed += 1

    print(f"\n{len(kept)} game(s) dumped in {out_dir}.")

    if args.push:
        print(f"{pushed} imported, {skipped} skipped (aborted), {failed} failed.")

    return 1 if failed else 0


if __name__ == "__main__":
    sys.exit(main())
