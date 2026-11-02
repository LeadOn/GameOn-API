// <copyright file="ValGameAdvancedStatCalculator.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameOn.Domain;

    /// <summary>
    /// Computes the round-level stats of <see cref="ValGameParticipantStat"/> that trackers rarely show: attack
    /// and defense split, pistol and buy-type rounds, trades, clutch attempts and kill distances. Runs after
    /// <see cref="ValGameParticipantStatCalculator.ComputeAll"/>, on the same in-memory match.
    /// </summary>
    public static class ValGameAdvancedStatCalculator
    {
        /// <summary>
        /// Loadout value under which a round counts as an eco.
        /// </summary>
        public const int EcoLoadoutCeiling = 2000;

        /// <summary>
        /// Loadout value from which a round counts as a full buy (rifle and heavy shields: 2,900 + 1,000).
        /// </summary>
        public const int FullBuyLoadoutFloor = 3900;

        private const int TradeWindowMs = 5000;

        private static readonly HashSet<string> StandardQueues = new (StringComparer.OrdinalIgnoreCase)
        {
            "competitive",
            "unrated",
            "premier",
        };

        /// <summary>
        /// Fills <see cref="ValGameRound.AttackingTeam"/> on every round where it can be told. From the most to
        /// the least reliable: the winning team and its role, the team that planted, then the other rounds of
        /// the same half (sides swap at half-time, then every overtime round).
        /// </summary>
        /// <param name="rounds">Rounds of the match.</param>
        /// <param name="participants">Participants of the match.</param>
        /// <param name="queueId">Normalized queue ID of the match.</param>
        public static void ResolveSides(IReadOnlyList<ValGameRound> rounds, IReadOnlyList<ValGameParticipant> participants, string? queueId)
        {
            var teams = participants
                .Select(p => p.TeamId)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (teams.Count != 2)
            {
                return;
            }

            var teamByPuuid = participants
                .Where(p => !string.IsNullOrWhiteSpace(p.TeamId))
                .GroupBy(p => p.Puuid, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().TeamId!, StringComparer.OrdinalIgnoreCase);

            string Other(string team) => string.Equals(team, teams[0], StringComparison.OrdinalIgnoreCase) ? teams[1] : teams[0];

            foreach (var round in rounds)
            {
                round.AttackingTeam = null;

                if (!string.IsNullOrWhiteSpace(round.WinningTeam) && !string.IsNullOrWhiteSpace(round.WinningTeamRole))
                {
                    round.AttackingTeam = string.Equals(round.WinningTeamRole, "Attacker", StringComparison.OrdinalIgnoreCase)
                        ? round.WinningTeam
                        : Other(round.WinningTeam);
                }
                else if (!string.IsNullOrWhiteSpace(round.PlantPlayerPuuid) && teamByPuuid.TryGetValue(round.PlantPlayerPuuid, out var planterTeam))
                {
                    round.AttackingTeam = planterTeam;
                }
            }

            var halfLength = HalfLength(queueId);

            if (halfLength is null)
            {
                return;
            }

            // Team attacking in phase 0 (first half, and every other overtime round), read off any known round.
            string? phaseZeroAttacker = null;

            foreach (var round in rounds.Where(r => r.AttackingTeam is not null))
            {
                phaseZeroAttacker = Phase(round.RoundNumber, halfLength.Value) == 0 ? round.AttackingTeam : Other(round.AttackingTeam!);
                break;
            }

            if (phaseZeroAttacker is null)
            {
                return;
            }

            foreach (var round in rounds.Where(r => r.AttackingTeam is null))
            {
                round.AttackingTeam = Phase(round.RoundNumber, halfLength.Value) == 0 ? phaseZeroAttacker : Other(phaseZeroAttacker);
            }
        }

        /// <summary>
        /// Computes the advanced stats of every participant, onto <see cref="ValGameParticipant.Stats"/>.
        /// </summary>
        /// <param name="participants">Participants, with <see cref="ValGameParticipant.Stats"/> already computed.</param>
        /// <param name="rounds">Rounds, with <see cref="ValGameRound.AttackingTeam"/> resolved.</param>
        /// <param name="kills">Kills of the match.</param>
        /// <param name="roundPlayerStats">Per-round player stats (loadout, score).</param>
        /// <param name="queueId">Normalized queue ID of the match.</param>
        public static void ComputeAll(
            IReadOnlyList<ValGameParticipant> participants,
            IReadOnlyList<ValGameRound> rounds,
            IReadOnlyList<ValGameKill> kills,
            IReadOnlyList<ValGameRoundPlayerStat> roundPlayerStats,
            string? queueId)
        {
            var teamByPuuid = participants
                .Where(p => !string.IsNullOrWhiteSpace(p.TeamId))
                .GroupBy(p => p.Puuid, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().TeamId!, StringComparer.OrdinalIgnoreCase);

            var killsByRound = kills
                .GroupBy(k => k.RoundNumber)
                .ToDictionary(g => g.Key, g => g.OrderBy(k => k.TimeInRoundMs).ThenBy(k => k.TimeInMatchMs).ToList());

            var roundStatByKey = roundPlayerStats
                .GroupBy(s => (s.RoundNumber, Puuid: s.Puuid.ToLowerInvariant()))
                .ToDictionary(g => g.Key, g => g.First());

            var clutches = FindClutches(rounds, killsByRound, roundPlayerStats, teamByPuuid);
            var halfLength = HalfLength(queueId);
            var isStandard = queueId is not null && StandardQueues.Contains(queueId);
            var sidesKnown = rounds.Count > 0 && rounds.All(r => r.AttackingTeam is not null);

            foreach (var participant in participants)
            {
                var stat = participant.Stats;

                if (stat is null || string.IsNullOrWhiteSpace(participant.TeamId))
                {
                    continue;
                }

                var puuid = participant.Puuid;
                var team = participant.TeamId;
                var myKills = kills.Where(k => Same(k.KillerPuuid, puuid) && !Same(k.VictimPuuid, puuid)).ToList();

                ComputeSides(stat, participant, rounds, killsByRound, roundStatByKey, sidesKnown);
                ComputeBuys(stat, participant, rounds, roundStatByKey, isStandard ? halfLength : null);

                stat.TradeKills = myKills.Count(k => IsTrade(k, killsByRound, teamByPuuid));
                stat.TradedDeaths = kills.Count(k => Same(k.VictimPuuid, puuid) && WasTraded(k, killsByRound, team, teamByPuuid));

                var myClutches = clutches.Where(c => Same(c.Puuid, puuid)).ToList();
                stat.ClutchAttempts = myClutches.Count;
                stat.Clutch1v1Won = myClutches.Count(c => c.Won && c.Opponents == 1);
                stat.Clutch1v2Won = myClutches.Count(c => c.Won && c.Opponents == 2);
                stat.Clutch1v3Won = myClutches.Count(c => c.Won && c.Opponents == 3);
                stat.Clutch1v4Won = myClutches.Count(c => c.Won && c.Opponents == 4);
                stat.Clutch1v5Won = myClutches.Count(c => c.Won && c.Opponents >= 5);

                var distances = myKills.Where(k => k.DistanceMeters is not null).Select(k => k.DistanceMeters!.Value).ToList();
                stat.AverageKillDistanceMeters = distances.Count > 0 ? Math.Round(distances.Average(), 1) : null;
                stat.LongestKillDistanceMeters = distances.Count > 0 ? Math.Round(distances.Max(), 1) : null;
            }
        }

        private static void ComputeSides(
            ValGameParticipantStat stat,
            ValGameParticipant participant,
            IReadOnlyList<ValGameRound> rounds,
            Dictionary<int, List<ValGameKill>> killsByRound,
            Dictionary<(int RoundNumber, string Puuid), ValGameRoundPlayerStat> roundStatByKey,
            bool sidesKnown)
        {
            if (!sidesKnown)
            {
                stat.AttackRounds = stat.AttackRoundsWon = stat.AttackKills = stat.AttackDeaths = stat.AttackScore = null;
                stat.DefenseRounds = stat.DefenseRoundsWon = stat.DefenseKills = stat.DefenseDeaths = stat.DefenseScore = null;
                return;
            }

            int attackRounds = 0, attackWon = 0, attackKills = 0, attackDeaths = 0, attackScore = 0;
            int defenseRounds = 0, defenseWon = 0, defenseKills = 0, defenseDeaths = 0, defenseScore = 0;
            var puuid = participant.Puuid;

            foreach (var round in rounds)
            {
                var roundKills = killsByRound.GetValueOrDefault(round.RoundNumber) ?? new List<ValGameKill>();
                var kills = roundKills.Count(k => Same(k.KillerPuuid, puuid) && !Same(k.VictimPuuid, puuid));
                var deaths = roundKills.Count(k => Same(k.VictimPuuid, puuid));
                var score = roundStatByKey.TryGetValue((round.RoundNumber, puuid.ToLowerInvariant()), out var rs) ? rs.Score : 0;
                var won = Same(round.WinningTeam, participant.TeamId);

                if (Same(round.AttackingTeam, participant.TeamId))
                {
                    attackRounds++;
                    attackWon += won ? 1 : 0;
                    attackKills += kills;
                    attackDeaths += deaths;
                    attackScore += score;
                }
                else
                {
                    defenseRounds++;
                    defenseWon += won ? 1 : 0;
                    defenseKills += kills;
                    defenseDeaths += deaths;
                    defenseScore += score;
                }
            }

            stat.AttackRounds = attackRounds;
            stat.AttackRoundsWon = attackWon;
            stat.AttackKills = attackKills;
            stat.AttackDeaths = attackDeaths;
            stat.AttackScore = attackScore;
            stat.DefenseRounds = defenseRounds;
            stat.DefenseRoundsWon = defenseWon;
            stat.DefenseKills = defenseKills;
            stat.DefenseDeaths = defenseDeaths;
            stat.DefenseScore = defenseScore;
        }

        private static void ComputeBuys(
            ValGameParticipantStat stat,
            ValGameParticipant participant,
            IReadOnlyList<ValGameRound> rounds,
            Dictionary<(int RoundNumber, string Puuid), ValGameRoundPlayerStat> roundStatByKey,
            int? halfLength)
        {
            if (halfLength is null || rounds.Count == 0)
            {
                stat.PistolRounds = stat.PistolRoundsWon = null;
                stat.EcoRounds = stat.EcoRoundsWon = null;
                stat.ForceBuyRounds = stat.ForceBuyRoundsWon = null;
                stat.FullBuyRounds = stat.FullBuyRoundsWon = null;
                return;
            }

            int pistol = 0, pistolWon = 0, eco = 0, ecoWon = 0, force = 0, forceWon = 0, full = 0, fullWon = 0;
            var key = participant.Puuid.ToLowerInvariant();

            foreach (var round in rounds)
            {
                var won = Same(round.WinningTeam, participant.TeamId);

                if (round.RoundNumber == 0 || round.RoundNumber == halfLength.Value)
                {
                    pistol++;
                    pistolWon += won ? 1 : 0;
                    continue;
                }

                if (!roundStatByKey.TryGetValue((round.RoundNumber, key), out var rs))
                {
                    continue;
                }

                if (rs.LoadoutValue < EcoLoadoutCeiling)
                {
                    eco++;
                    ecoWon += won ? 1 : 0;
                }
                else if (rs.LoadoutValue < FullBuyLoadoutFloor)
                {
                    force++;
                    forceWon += won ? 1 : 0;
                }
                else
                {
                    full++;
                    fullWon += won ? 1 : 0;
                }
            }

            stat.PistolRounds = pistol;
            stat.PistolRoundsWon = pistolWon;
            stat.EcoRounds = eco;
            stat.EcoRoundsWon = ecoWon;
            stat.ForceBuyRounds = force;
            stat.ForceBuyRoundsWon = forceWon;
            stat.FullBuyRounds = full;
            stat.FullBuyRoundsWon = fullWon;
        }

        /// <summary>
        /// Finds, for each round and each team, the player left alone against at least one enemy, with the
        /// number of enemies alive at that moment. Both teams can clutch in the same round: a 1v2 that turns
        /// into a 1v1 is a clutch for each of the two last players.
        /// </summary>
        private static List<(string Puuid, int Opponents, bool Won)> FindClutches(
            IReadOnlyList<ValGameRound> rounds,
            Dictionary<int, List<ValGameKill>> killsByRound,
            IReadOnlyList<ValGameRoundPlayerStat> roundPlayerStats,
            Dictionary<string, string> teamByPuuid)
        {
            var result = new List<(string Puuid, int Opponents, bool Won)>();
            var roundPlayers = roundPlayerStats.ToLookup(s => s.RoundNumber);

            foreach (var round in rounds)
            {
                // Players present in the round: the round stats when they exist (they leave out disconnected
                // players), every participant otherwise.
                var present = roundPlayers[round.RoundNumber].Select(s => s.Puuid).ToList();

                if (present.Count == 0)
                {
                    present = teamByPuuid.Keys.ToList();
                }

                var alive = present
                    .Where(teamByPuuid.ContainsKey)
                    .GroupBy(p => teamByPuuid[p], StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => new HashSet<string>(g, StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);

                if (alive.Count != 2)
                {
                    continue;
                }

                var clutchingTeams = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var kill in killsByRound.GetValueOrDefault(round.RoundNumber) ?? new List<ValGameKill>())
                {
                    if (string.IsNullOrWhiteSpace(kill.VictimPuuid))
                    {
                        continue;
                    }

                    foreach (var side in alive.Values)
                    {
                        side.Remove(kill.VictimPuuid);
                    }

                    foreach (var lastStanding in alive.Where(x => x.Value.Count == 1 && !clutchingTeams.Contains(x.Key)).ToList())
                    {
                        var opponents = alive.Where(x => !string.Equals(x.Key, lastStanding.Key, StringComparison.OrdinalIgnoreCase)).Sum(x => x.Value.Count);

                        if (opponents >= 1)
                        {
                            clutchingTeams.Add(lastStanding.Key);
                            result.Add((lastStanding.Value.First(), opponents, Same(round.WinningTeam, lastStanding.Key)));
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsTrade(ValGameKill kill, Dictionary<int, List<ValGameKill>> killsByRound, Dictionary<string, string> teamByPuuid)
        {
            if (string.IsNullOrWhiteSpace(kill.KillerPuuid) || !teamByPuuid.TryGetValue(kill.KillerPuuid, out var killerTeam))
            {
                return false;
            }

            // The victim had just killed a teammate of the killer.
            return (killsByRound.GetValueOrDefault(kill.RoundNumber) ?? new List<ValGameKill>()).Any(earlier =>
                Same(earlier.KillerPuuid, kill.VictimPuuid)
                && earlier.VictimPuuid is not null
                && teamByPuuid.TryGetValue(earlier.VictimPuuid, out var victimTeam)
                && Same(victimTeam, killerTeam)
                && earlier.TimeInRoundMs <= kill.TimeInRoundMs
                && kill.TimeInRoundMs - earlier.TimeInRoundMs <= TradeWindowMs
                && !ReferenceEquals(earlier, kill));
        }

        private static bool WasTraded(ValGameKill death, Dictionary<int, List<ValGameKill>> killsByRound, string team, Dictionary<string, string> teamByPuuid)
        {
            if (string.IsNullOrWhiteSpace(death.KillerPuuid))
            {
                return false;
            }

            return (killsByRound.GetValueOrDefault(death.RoundNumber) ?? new List<ValGameKill>()).Any(later =>
                Same(later.VictimPuuid, death.KillerPuuid)
                && later.KillerPuuid is not null
                && teamByPuuid.TryGetValue(later.KillerPuuid, out var avengerTeam)
                && Same(avengerTeam, team)
                && later.TimeInRoundMs >= death.TimeInRoundMs
                && later.TimeInRoundMs - death.TimeInRoundMs <= TradeWindowMs);
        }

        /// <summary>
        /// Rounds per half, null when the mode's halves are unknown.
        /// </summary>
        private static int? HalfLength(string? queueId) => queueId?.ToLowerInvariant() switch
        {
            "competitive" or "unrated" or "premier" => 12,
            "swiftplay" => 4,
            _ => null,
        };

        /// <summary>
        /// 0 for the first half (and every other overtime round from the first one), 1 otherwise.
        /// </summary>
        private static int Phase(int roundNumber, int halfLength)
        {
            if (roundNumber < halfLength)
            {
                return 0;
            }

            if (roundNumber < 2 * halfLength)
            {
                return 1;
            }

            return (roundNumber - (2 * halfLength)) % 2;
        }

        private static bool Same(string? a, string? b) =>
            a is not null && b is not null && string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    }
}
