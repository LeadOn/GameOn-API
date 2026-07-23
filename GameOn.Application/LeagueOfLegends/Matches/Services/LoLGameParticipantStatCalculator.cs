// <copyright file="LoLGameParticipantStatCalculator.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Services
{
    using GameOn.Domain;

    /// <summary>
    /// Computes derived <see cref="LoLGameParticipantStat"/> values from already-loaded match data.
    /// Shared between the live import path (<c>UpdateLoLGameCommandHandler</c>) and the bulk backfill
    /// command, so both stay in sync on the exact same formulas.
    /// </summary>
    public static class LoLGameParticipantStatCalculator
    {
        /// <summary>
        /// Computes the derived performance stats for a single participant.
        /// </summary>
        /// <param name="participant">The participant to compute stats for.</param>
        /// <param name="teamKills">The total kills of the participant's team, for kill participation.</param>
        /// <param name="lastFrame">The participant's last timeline frame (cumulative end-of-game values), if any.</param>
        /// <param name="lastFrameTimestampMs">The timestamp (ms) of the match's last timeline frame, used as the game duration.</param>
        /// <param name="wardsPlaced">Number of wards placed by the participant, from timeline events.</param>
        /// <param name="wardsKilled">Number of wards killed by the participant, from timeline events.</param>
        /// <param name="target">An existing <see cref="LoLGameParticipantStat"/> to update in place, if any; a new one is created otherwise.</param>
        /// <returns>The computed <see cref="LoLGameParticipantStat"/> (either <paramref name="target"/>, updated, or a new instance).</returns>
        public static LoLGameParticipantStat Compute(
            LoLGameParticipant participant,
            int teamKills,
            LoLGameTimelineFrameParticipant? lastFrame,
            int lastFrameTimestampMs,
            int wardsPlaced,
            int wardsKilled,
            LoLGameParticipantStat? target = null)
        {
            var creepScore = lastFrame is null ? 0 : lastFrame.MinionsKilled + lastFrame.JungleMinionsKilled;
            var goldEarned = lastFrame?.TotalGold ?? 0;
            var damageDealtToChampions = lastFrame?.TotalDamageDoneToChampions ?? 0;
            var durationMinutes = lastFrameTimestampMs / 60000.0;

            var stat = target ?? new LoLGameParticipantStat();

            stat.LoLGameParticipantId = participant.Id;
            stat.GameDurationSeconds = lastFrameTimestampMs / 1000;
            stat.Kda = Math.Round((participant.Kills + participant.Assists) / (double)Math.Max(participant.Deaths, 1), 2);
            stat.KillParticipationPercent = teamKills > 0 ? Math.Round(100.0 * (participant.Kills + participant.Assists) / teamKills, 1) : 0;
            stat.CreepScore = creepScore;
            stat.CsPerMinute = durationMinutes > 0 ? Math.Round(creepScore / durationMinutes, 2) : 0;
            stat.GoldEarned = goldEarned;
            stat.GoldPerMinute = durationMinutes > 0 ? Math.Round(goldEarned / durationMinutes, 2) : 0;
            stat.DamageDealtToChampions = damageDealtToChampions;
            stat.DamagePerMinute = durationMinutes > 0 ? Math.Round(damageDealtToChampions / durationMinutes, 2) : 0;
            stat.DamageTaken = lastFrame?.TotalDamageTaken ?? 0;
            stat.WardsPlaced = wardsPlaced;
            stat.WardsKilled = wardsKilled;
            stat.ComputedOn = DateTime.UtcNow;

            return stat;
        }
    }
}
