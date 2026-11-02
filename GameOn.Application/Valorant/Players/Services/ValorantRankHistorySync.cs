// <copyright file="ValorantRankHistorySync.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Models.DTOs;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Writes HenrikDev MMR history entries into <see cref="ValorantRankHistory"/>. Shared by the regular
    /// refresh (mmr-history v2, last ~20 matches) and the backfill (stored-mmr-history v2).
    /// </summary>
    public static class ValorantRankHistorySync
    {
        /// <summary>
        /// Inserts the entries not known yet and completes the ones already stored (an entry first written
        /// before the before-match detail existed gets it on the next pass), then recomputes every
        /// <see cref="ValorantRankHistory.RrDelta"/> of the player and copies it onto their match participations.
        /// </summary>
        /// <param name="context">DbContext.</param>
        /// <param name="playerId">Player the entries belong to.</param>
        /// <param name="entries">HenrikDev entries, in any order.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The player's rank entries, oldest first, tracked by the context.</returns>
        public static async Task<List<ValorantRankHistory>> Upsert(
            IApplicationDbContext context,
            int playerId,
            IEnumerable<ValorantMmrHistoryEntryDto> entries,
            CancellationToken cancellationToken)
        {
            var stored = await context.ValorantRankHistory
                .Where(x => x.PlayerId == playerId)
                .ToListAsync(cancellationToken);

            var byMatchId = stored
                .Where(x => !string.IsNullOrWhiteSpace(x.MatchId))
                .GroupBy(x => x.MatchId!, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Id).First(), StringComparer.OrdinalIgnoreCase);

            // Inserted oldest first so that IDs stay ordered with CreatedOn.
            foreach (var entry in entries.Where(x => !string.IsNullOrWhiteSpace(x.MatchId)).OrderBy(x => ParseUtcDate(x.Date)))
            {
                if (!byMatchId.TryGetValue(entry.MatchId, out var row))
                {
                    row = new ValorantRankHistory { PlayerId = playerId, MatchId = entry.MatchId };
                    context.ValorantRankHistory.Add(row);
                    stored.Add(row);
                    byMatchId[entry.MatchId] = row;
                }

                Apply(row, entry);
            }

            ValorantRrDeltaCalculator.FillAll(stored);
            await SyncParticipants(context, playerId, stored, cancellationToken);

            return stored.OrderBy(x => x.CreatedOn).ThenBy(x => x.Id).ToList();
        }

        /// <summary>
        /// Parses a HenrikDev ISO 8601 date as UTC.
        /// </summary>
        /// <param name="rawDate">Raw date.</param>
        /// <returns>The UTC date, or now when unreadable.</returns>
        public static DateTime ParseUtcDate(string? rawDate)
        {
            if (!string.IsNullOrWhiteSpace(rawDate)
                && DateTimeOffset.TryParse(rawDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var parsed))
            {
                return parsed.UtcDateTime;
            }

            return DateTime.UtcNow;
        }

        /// <summary>
        /// Copies the post-match rank of an entry onto the matching participation.
        /// </summary>
        /// <param name="participant">Participation to update.</param>
        /// <param name="rank">Rank entry of the same player and match.</param>
        public static void ApplyToParticipant(ValGameParticipant participant, ValorantRankHistory rank)
        {
            participant.RrChange = rank.RrDelta ?? rank.LastChange;
            participant.PostGameTierId = rank.TierId;
            participant.PostGameTierName = rank.TierName;
            participant.PostGameRankedRating = rank.RankedRating;
        }

        private static void Apply(ValorantRankHistory row, ValorantMmrHistoryEntryDto entry)
        {
            row.SeasonId = entry.Season?.Id;
            row.SeasonShort = entry.Season?.Short;
            row.TierId = entry.Tier?.Id ?? 0;
            row.TierName = entry.Tier?.Name ?? "Unranked";
            row.RankedRating = entry.Rr;
            row.Elo = entry.Elo;
            row.LastChange = entry.LastChange;
            row.RefundedRr = entry.RefundedRr;
            row.WasDerankProtected = entry.WasDerankProtected;
            row.MapId = entry.Map?.Id;
            row.MapName = entry.Map?.Name;
            row.CreatedOn = ParseUtcDate(entry.Date);

            // Old stored records carry "" and 0 where the value was simply not recorded yet.
            row.QueueId = string.IsNullOrWhiteSpace(entry.QueueId) ? null : entry.QueueId;
            row.MatchLengthMs = entry.MatchLength is > 0 ? entry.MatchLength : null;
            row.TierBeforeId = entry.TierBeforeUpdate?.Id;
            row.TierBeforeName = entry.TierBeforeUpdate?.Name;
            row.RrBeforeUpdate = entry.RrBeforeUpdate;
            row.RrPerformanceBonus = entry.RrPerformanceBonus;
            row.RrPenalty = entry.RrPenalty;
            row.AfkPenalty = entry.AfkPenalty;
            row.NewMapIncentiveRrForgiven = entry.NewMapIncentiveRrForgiven;
            row.IsPlacementMatch = entry.IsPlacementMatch;
            row.WasDerankProtectionReplenished = entry.WasDerankProtectionReplenished;
        }

        private static async Task SyncParticipants(
            IApplicationDbContext context,
            int playerId,
            List<ValorantRankHistory> stored,
            CancellationToken cancellationToken)
        {
            var rankByMatchId = stored
                .Where(x => !string.IsNullOrWhiteSpace(x.MatchId))
                .GroupBy(x => x.MatchId!, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Id).First(), StringComparer.OrdinalIgnoreCase);

            if (rankByMatchId.Count == 0)
            {
                return;
            }

            // Every participation of the player, matched in memory: a list of match IDs in the query would hit
            // SQL Server's 2,100 parameter limit on a long history.
            var participants = await context.ValorantGameParticipants
                .Where(x => x.PlayerId == playerId)
                .ToListAsync(cancellationToken);

            foreach (var participant in participants)
            {
                if (rankByMatchId.TryGetValue(participant.MatchId, out var rank))
                {
                    ApplyToParticipant(participant, rank);
                }
            }
        }
    }
}
