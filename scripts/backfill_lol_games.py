#!/usr/bin/env python3
"""Backfill LoL games: re-sync every game from the Riot Games API.

Games imported before 2026-07-11 are missing ping/bounty/consumable data
(the import mapping didn't copy those fields), and ~900 games only contain
a placeholder participant left by failed imports. Re-syncing each game via
POST /lol/Match/{matchId}/update fixes both.

Each update triggers 2 Riot API calls (match + timeline). A dev API key
allows 100 calls / 2 minutes, hence the default 3s delay between games.

Usage:
    python backfill_lol_games.py                     # full run (resumable)
    python backfill_lol_games.py --limit 5           # test on 5 games
    python backfill_lol_games.py --base-url http://localhost:5184 --delay 3

Progress is saved to backfill_done.txt after each game: re-running the
script skips games already done and retries only failures and new games.
Failed match IDs are logged to backfill_failed.txt (overwritten each run).
"""

import argparse
import json
import sys
import time
import urllib.error
import urllib.request
from pathlib import Path

SCRIPT_DIR = Path(__file__).resolve().parent
DONE_FILE = SCRIPT_DIR / "backfill_done.txt"
FAILED_FILE = SCRIPT_DIR / "backfill_failed.txt"


def http_json(url: str, method: str = "GET", timeout: int = 60):
    req = urllib.request.Request(url, method=method)
    with urllib.request.urlopen(req, timeout=timeout) as resp:
        body = resp.read()
        return json.loads(body) if body else None


def fetch_all_match_ids(base_url: str, page_size: int = 100) -> list[str]:
    ids: list[str] = []
    page = 1
    while True:
        data = http_json(f"{base_url}/lol/Match/last?page={page}&size={page_size}")
        results = data.get("results") or []
        if not results:
            break
        ids.extend(g["matchId"] for g in results)
        total = data.get("total", 0)
        print(f"\rListing games... {len(ids)}/{total}", end="", flush=True)
        if len(ids) >= total:
            break
        page += 1
    print()
    return ids


def main() -> int:
    parser = argparse.ArgumentParser(description="Re-sync all LoL games from Riot.")
    parser.add_argument("--base-url", default="http://localhost:5184")
    parser.add_argument("--delay", type=float, default=3.0, help="seconds between games (default: 3)")
    parser.add_argument("--limit", type=int, default=None, help="only process N games (for testing)")
    parser.add_argument("--cooldown", type=float, default=120.0, help="pause after repeated failures (default: 120s)")
    args = parser.parse_args()
    base_url = args.base_url.rstrip("/")

    try:
        all_ids = fetch_all_match_ids(base_url)
    except urllib.error.URLError as e:
        print(f"Cannot reach the API at {base_url} — is it running? ({e})")
        return 1

    done = set()
    if DONE_FILE.exists():
        done = {line.strip() for line in DONE_FILE.read_text().splitlines() if line.strip()}

    todo = [m for m in all_ids if m not in done]
    if args.limit is not None:
        todo = todo[: args.limit]

    print(f"{len(all_ids)} games in database, {len(done)} already done, {len(todo)} to process.")
    if not todo:
        print("Nothing to do.")
        return 0

    eta_min = len(todo) * args.delay / 60
    print(f"Estimated duration: ~{eta_min:.0f} min at {args.delay}s per game.\n")

    ok, ko = 0, 0
    consecutive_failures = 0
    failed: list[str] = []
    start = time.monotonic()

    for i, match_id in enumerate(todo, 1):
        try:
            http_json(f"{base_url}/lol/Match/{match_id}/update", method="POST")
            ok += 1
            consecutive_failures = 0
            with DONE_FILE.open("a") as f:
                f.write(match_id + "\n")
            status = "OK"
        except (urllib.error.URLError, TimeoutError) as e:
            ko += 1
            consecutive_failures += 1
            failed.append(match_id)
            reason = getattr(e, "code", e)
            status = f"FAILED ({reason})"

        elapsed = time.monotonic() - start
        remaining = (len(todo) - i) * (elapsed / i)
        print(f"[{i}/{len(todo)}] {match_id}: {status} | ok={ok} ko={ko} | ETA {remaining / 60:.0f} min")

        # Repeated failures usually mean the Riot rate limit kicked in: cool down.
        if consecutive_failures >= 5:
            print(f"5 failures in a row — cooling down {args.cooldown:.0f}s (rate limit?)...")
            time.sleep(args.cooldown)
            consecutive_failures = 0
        elif i < len(todo):
            time.sleep(args.delay)

    if failed:
        FAILED_FILE.write_text("\n".join(failed) + "\n")
        print(f"\n{ko} failures logged to {FAILED_FILE.name} — re-run the script to retry them.")
        print("(Games no longer available on the Riot API will keep failing: this is expected for very old matches.)")

    print(f"\nDone: {ok} games re-synced, {ko} failures, in {(time.monotonic() - start) / 60:.1f} min.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
