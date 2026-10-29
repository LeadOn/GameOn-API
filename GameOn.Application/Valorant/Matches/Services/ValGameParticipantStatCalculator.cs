// <copyright file="ValGameParticipantStatCalculator.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameOn.Domain;

    /// <summary>
    /// Computes derived <see cref="ValGameParticipantStat"/> metrics (ACS, ADR, KAST%, First Kills/Deaths,
    /// Multi-kills, Clutches, Plants/Defuses, Econ Rating, and overall GameOn! Rating) from a match's
    /// participants, rounds, and kill timeline.
    /// </summary>
    public static class ValGameParticipantStatCalculator
    {
        private const int TradeWindowMs = 5000;

        /// <summary>
        /// Computes and assigns <see cref="ValGameParticipant.Stats"/> for every participant in the match.
        /// </summary>
        /// <param name="participants">All participants of the match (with <see cref="ValGameParticipant.Id"/> populated).</param>
        /// <param name="rounds">All rounds of the match.</param>
        /// <param name="kills">All kill events of the match.</param>
        public static void ComputeAll(
            IReadOnlyList<ValGameParticipant> participants,
            IReadOnlyList<ValGameRound> rounds,
            IReadOnlyList<ValGameKill> kills)
        {
            var roundsPlayed = rounds.Count;
            var killsByRound = kills
                .GroupBy(k => k.RoundNumber)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(k => k.TimeInRoundMs).ThenBy(k => k.TimeInMatchMs).ThenBy(k => k.Id).ToList());

            var firstKillCountByPuuid = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var firstDeathCountByPuuid = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var roundKills in killsByRound.Values)
            {
                var firstKill = roundKills.FirstOrDefault(k =>
                    !string.IsNullOrWhiteSpace(k.KillerPuuid)
                    && !string.IsNullOrWhiteSpace(k.VictimPuuid)
                    && !string.Equals(k.KillerPuuid, k.VictimPuuid, StringComparison.OrdinalIgnoreCase));

                if (firstKill is not null)
                {
                    firstKillCountByPuuid[firstKill.KillerPuuid!] = firstKillCountByPuuid.GetValueOrDefault(firstKill.KillerPuuid!) + 1;
                    firstDeathCountByPuuid[firstKill.VictimPuuid!] = firstDeathCountByPuuid.GetValueOrDefault(firstKill.VictimPuuid!) + 1;
                }
            }

            var teamKillsByTeamId = participants
                .Where(p => !string.IsNullOrWhiteSpace(p.TeamId))
                .GroupBy(p => p.TeamId!, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.Kills), StringComparer.OrdinalIgnoreCase);

            foreach (var participant in participants)
            {
                var puuid = participant.Puuid;
                var teamKills = !string.IsNullOrWhiteSpace(participant.TeamId)
                    ? teamKillsByTeamId.GetValueOrDefault(participant.TeamId, 0)
                    : 0;

                var stat = participant.Stats ?? new ValGameParticipantStat();
                stat.ValGameParticipantId = participant.Id;
                stat.RoundsPlayed = roundsPlayed;
                stat.Acs = roundsPlayed > 0 ? Math.Round((double)participant.Score / roundsPlayed, 1) : participant.Score;
                stat.Kda = Math.Round((participant.Kills + participant.Assists) / (double)Math.Max(participant.Deaths, 1), 2);
                stat.KdRatio = Math.Round(participant.Kills / (double)Math.Max(participant.Deaths, 1), 2);
                stat.KillParticipationPercent = teamKills > 0
                    ? Math.Round(100.0 * (participant.Kills + participant.Assists) / teamKills, 1)
                    : 0;
                stat.Adr = roundsPlayed > 0 ? Math.Round((double)participant.DamageDealt / roundsPlayed, 1) : 0;
                stat.DamageDeltaPerRound = roundsPlayed > 0
                    ? Math.Round((double)(participant.DamageDealt - participant.DamageReceived) / roundsPlayed, 1)
                    : 0;

                var totalShots = participant.Headshots + participant.Bodyshots + participant.Legshots;
                stat.HeadshotPercent = totalShots > 0
                    ? Math.Round(100.0 * participant.Headshots / totalShots, 1)
                    : 0;

                stat.FirstKills = firstKillCountByPuuid.GetValueOrDefault(puuid, 0);
                stat.FirstDeaths = firstDeathCountByPuuid.GetValueOrDefault(puuid, 0);

                var doubleKills = 0;
                var tripleKills = 0;
                var quadraKills = 0;
                var aces = 0;
                var clutchesWon = 0;
                var spikePlants = 0;
                var spikeDefuses = 0;
                var kastRounds = 0;

                foreach (var round in rounds)
                {
                    if (string.Equals(round.PlantPlayerPuuid, puuid, StringComparison.OrdinalIgnoreCase))
                    {
                        spikePlants++;
                    }

                    if (string.Equals(round.DefusePlayerPuuid, puuid, StringComparison.OrdinalIgnoreCase))
                    {
                        spikeDefuses++;
                    }

                    if (string.Equals(round.Ceremony, "CeremonyClutch", StringComparison.OrdinalIgnoreCase)
                        && string.Equals(round.WinningTeam, participant.TeamId, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(round.CeremonyPlayerPuuid, puuid, StringComparison.OrdinalIgnoreCase))
                    {
                        clutchesWon++;
                    }

                    var roundKills = killsByRound.TryGetValue(round.RoundNumber, out var rk)
                        ? rk
                        : new List<ValGameKill>();

                    var playerKillsInRound = roundKills.Count(k =>
                        string.Equals(k.KillerPuuid, puuid, StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(k.VictimPuuid, puuid, StringComparison.OrdinalIgnoreCase));

                    if (playerKillsInRound == 2)
                    {
                        doubleKills++;
                    }
                    else if (playerKillsInRound == 3)
                    {
                        tripleKills++;
                    }
                    else if (playerKillsInRound == 4)
                    {
                        quadraKills++;
                    }
                    else if (playerKillsInRound >= 5
                        || (string.Equals(round.Ceremony, "CeremonyAce", StringComparison.OrdinalIgnoreCase)
                            && string.Equals(round.CeremonyPlayerPuuid, puuid, StringComparison.OrdinalIgnoreCase)))
                    {
                        aces++;
                    }

                    var hadKill = playerKillsInRound > 0;
                    var hadAssist = roundKills.Any(k => ContainsAssistant(k.AssistantPuuids, puuid));
                    var deathEvents = roundKills
                        .Where(k => string.Equals(k.VictimPuuid, puuid, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    var survived = deathEvents.Count == 0;
                    var wasTraded = !survived && deathEvents.Any(death =>
                        !string.IsNullOrWhiteSpace(death.KillerPuuid)
                        && roundKills.Any(trade =>
                            string.Equals(trade.VictimPuuid, death.KillerPuuid, StringComparison.OrdinalIgnoreCase)
                            && trade.TimeInRoundMs >= death.TimeInRoundMs
                            && trade.TimeInRoundMs - death.TimeInRoundMs <= TradeWindowMs));

                    if (hadKill || hadAssist || survived || wasTraded)
                    {
                        kastRounds++;
                    }
                }

                stat.DoubleKills = doubleKills;
                stat.TripleKills = tripleKills;
                stat.QuadraKills = quadraKills;
                stat.Aces = aces;
                stat.ClutchesWon = clutchesWon;
                stat.SpikePlants = spikePlants;
                stat.SpikeDefuses = spikeDefuses;
                stat.KnifeKills = kills.Count(k =>
                    string.Equals(k.KillerPuuid, puuid, StringComparison.OrdinalIgnoreCase)
                    && (string.Equals(k.WeaponType, "Melee", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(k.WeaponName, "Melee", StringComparison.OrdinalIgnoreCase)));
                stat.KastPercent = roundsPlayed > 0
                    ? Math.Round(100.0 * kastRounds / roundsPlayed, 1)
                    : 0;
                stat.EconRating = participant.SpentOverall > 0
                    ? Math.Round(participant.DamageDealt / (double)participant.SpentOverall * 1000.0, 1)
                    : 0;

                var acsPart = Clamp01(stat.Acs / 300.0);
                var adrPart = Clamp01(stat.Adr / 180.0);
                var kastPart = Clamp01(stat.KastPercent / 85.0);
                var kdaPart = Clamp01(stat.Kda / 3.0);
                var entryPart = Clamp01(0.5 + ((stat.FirstKills - stat.FirstDeaths) * 0.1));
                var hsPart = Clamp01(stat.HeadshotPercent / 35.0);

                var rating = (10.0 * ((0.30 * acsPart) + (0.20 * adrPart) + (0.20 * kastPart) + (0.15 * kdaPart) + (0.10 * entryPart) + (0.05 * hsPart)))
                    + (participant.Win ? 0.4 : 0);
                stat.Rating = Math.Round(Math.Min(10.0, Math.Max(0.0, rating)), 2);
                stat.ComputedOn = DateTime.UtcNow;

                participant.Stats = stat;
            }
        }

        /// <summary>
        /// Determines the MVP (winning team) and ACE (losing team) participant IDs for a Valorant match.
        /// </summary>
        /// <param name="participants">All participants of the match with <see cref="ValGameParticipant.Stats"/> computed.</param>
        /// <param name="winningTeamId">The winning team ID, if any.</param>
        /// <param name="isRemake">Whether the match was a remake.</param>
        /// <returns>The MVP and ACE participant IDs.</returns>
        public static (int? MvpParticipantId, int? AceParticipantId) DetermineMvpAndAce(
            IReadOnlyList<ValGameParticipant> participants,
            string? winningTeamId,
            bool isRemake)
        {
            if (isRemake || string.IsNullOrWhiteSpace(winningTeamId))
            {
                return (null, null);
            }

            var mvp = participants
                .Where(p => string.Equals(p.TeamId, winningTeamId, StringComparison.OrdinalIgnoreCase) && p.Stats is not null)
                .OrderByDescending(p => p.Stats!.Rating)
                .ThenByDescending(p => p.Score)
                .ThenByDescending(p => p.Kills)
                .FirstOrDefault();

            var ace = participants
                .Where(p => !string.Equals(p.TeamId, winningTeamId, StringComparison.OrdinalIgnoreCase) && p.Stats is not null)
                .OrderByDescending(p => p.Stats!.Rating)
                .ThenByDescending(p => p.Score)
                .ThenByDescending(p => p.Kills)
                .FirstOrDefault();

            return (mvp?.Id, ace?.Id);
        }

        private static bool ContainsAssistant(string? assistantPuuids, string puuid)
        {
            if (string.IsNullOrWhiteSpace(assistantPuuids))
            {
                return false;
            }

            foreach (var part in assistantPuuids.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (string.Equals(part, puuid, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static double Clamp01(double value)
            => double.IsFinite(value) ? Math.Min(1.0, Math.Max(0.0, value)) : 0.0;
    }
}
