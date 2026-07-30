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
        /// <param name="teamDamage">The total damage dealt to champions of the participant's team, for the damage share used in <see cref="LoLGameParticipantStat.Rating"/>.</param>
        /// <param name="lastFrame">The participant's last timeline frame (cumulative end-of-game values), if any.</param>
        /// <param name="lastFrameTimestampMs">The timestamp (ms) of the match's last timeline frame, used as the game duration.</param>
        /// <param name="wardsPlaced">Number of wards placed by the participant, from timeline events.</param>
        /// <param name="wardsKilled">Number of wards killed by the participant, from timeline events.</param>
        /// <param name="target">An existing <see cref="LoLGameParticipantStat"/> to update in place, if any; a new one is created otherwise.</param>
        /// <returns>The computed <see cref="LoLGameParticipantStat"/> (either <paramref name="target"/>, updated, or a new instance).</returns>
        public static LoLGameParticipantStat Compute(
            LoLGameParticipant participant,
            int teamKills,
            int teamDamage,
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

            // Riot computes KDA and kill participation itself (via the match "challenges" object) once a
            // match has been (re)imported after that field was added; no need to recompute it ourselves
            // when it's available. Older/not-yet-resynced participants fall back to the manual formula.
            stat.LoLGameParticipantId = participant.Id;
            stat.GameDurationSeconds = lastFrameTimestampMs / 1000;
            stat.Kda = participant.Challenges is not null
                ? Math.Round(participant.Challenges.Kda, 2)
                : Math.Round((participant.Kills + participant.Assists) / (double)Math.Max(participant.Deaths, 1), 2);
            stat.KillParticipationPercent = participant.Challenges is not null
                ? Math.Round(100.0 * participant.Challenges.KillParticipation, 1)
                : teamKills > 0 ? Math.Round(100.0 * (participant.Kills + participant.Assists) / teamKills, 1) : 0;
            stat.CreepScore = creepScore;
            stat.CsPerMinute = durationMinutes > 0 ? Math.Round(creepScore / durationMinutes, 2) : 0;
            stat.GoldEarned = goldEarned;
            stat.GoldPerMinute = durationMinutes > 0 ? Math.Round(goldEarned / durationMinutes, 2) : 0;
            stat.DamageDealtToChampions = damageDealtToChampions;
            stat.DamagePerMinute = durationMinutes > 0 ? Math.Round(damageDealtToChampions / durationMinutes, 2) : 0;
            stat.DamageTaken = lastFrame?.TotalDamageTaken ?? 0;
            stat.WardsPlaced = wardsPlaced;
            stat.WardsKilled = wardsKilled;
            stat.PhysicalDamageToChampions = lastFrame?.PhysicalDamageDoneToChampions ?? 0;
            stat.MagicDamageToChampions = lastFrame?.MagicDamageDoneToChampions ?? 0;
            stat.TrueDamageToChampions = lastFrame?.TrueDamageDoneToChampions ?? 0;
            stat.TimeCcOthersSeconds = lastFrame is null ? 0 : (int)Math.Round(lastFrame.TimeEnemySpentControlled / 1000.0);
            stat.ComputedOn = DateTime.UtcNow;

            var damageShare = participant.Challenges is not null
                ? (double)participant.Challenges.TeamDamagePercentage
                : teamDamage > 0 ? (double)damageDealtToChampions / teamDamage : 0;

            var kdaPart = Clamp01(stat.Kda / 6.0);
            var kpPart = Clamp01(stat.KillParticipationPercent / 65.0);
            var damagePart = Clamp01(damageShare / 0.3);
            var goldPart = Clamp01(stat.GoldPerMinute / 500.0);
            var survivalPart = Clamp01(1.0 - (participant.Deaths / 12.0));

            var rating = (10.0 * ((0.25 * kdaPart) + (0.20 * kpPart) + (0.25 * damagePart) + (0.15 * goldPart) + (0.15 * survivalPart)))
                + (participant.Win ? 0.4 : 0);
            stat.Rating = Math.Round(Math.Min(10.0, Math.Max(0.0, rating)), 2);

            return stat;
        }

        /// <summary>
        /// Determines the MVP (winning team) and ACE (losing team) participants for a match, based on
        /// their already-computed <see cref="LoLGameParticipantStat.Rating"/>. Both are null on a remake,
        /// when there's no winning team, or when ratings aren't computable.
        /// </summary>
        /// <param name="participants">All participants of the match, with <see cref="LoLGameParticipant.Stats"/> already computed.</param>
        /// <param name="winningTeamId">The match's winning team ID, if any.</param>
        /// <param name="isRemake">Whether the match ended in an early surrender for all participants.</param>
        /// <returns>The MVP and ACE participant IDs.</returns>
        public static (int? MvpParticipantId, int? AceParticipantId) DetermineMvpAndAce(
            IReadOnlyList<LoLGameParticipant> participants,
            int? winningTeamId,
            bool isRemake)
        {
            if (isRemake || winningTeamId is null)
            {
                return (null, null);
            }

            var mvp = participants
                .Where(p => p.TeamId == winningTeamId && p.Stats is not null)
                .OrderByDescending(p => p.Stats!.Rating)
                .FirstOrDefault();

            var ace = participants
                .Where(p => p.TeamId != winningTeamId && p.Stats is not null)
                .OrderByDescending(p => p.Stats!.Rating)
                .FirstOrDefault();

            return (mvp?.Id, ace?.Id);
        }

        private static double Clamp01(double value)
            => double.IsFinite(value) ? Math.Min(1.0, Math.Max(0.0, value)) : 0.0;
    }
}
