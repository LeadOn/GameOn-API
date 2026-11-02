// <copyright file="ValorantRrDeltaCalculator.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using GameOn.Domain;

    /// <summary>
    /// Computes the RR a ranked match really moved (<see cref="ValorantRankHistory.RrDelta"/>).
    /// </summary>
    /// <remarks>
    /// Riot's <c>last_change</c> is the change it announces, not the one applied: a loss absorbed at 0 RR is
    /// announced -22 while the RR only drops from 4 to 0, and a promotion announced +16 really gave +22. Over
    /// 20 games of one player the announced changes added up to -101 RR for a real -43. Below Immortal the Elo
    /// is <c>(tier - 3) * 100 + RR</c>, so the real change is the Elo after the match minus the Elo rebuilt from
    /// the tier and RR held before it.
    /// </remarks>
    public static class ValorantRrDeltaCalculator
    {
        /// <summary>
        /// First tier ID of the ranked ladder (Iron 1).
        /// </summary>
        private const int FirstRankedTier = 3;

        /// <summary>
        /// First tier ID where the Elo formula is not verified (Immortal 1): no account of the crew has been
        /// there to check it against.
        /// </summary>
        private const int FirstImmortalTier = 24;

        /// <summary>
        /// Computes the RR moved by one entry from the tier and RR held before it.
        /// </summary>
        /// <param name="entry">Rank history entry, with its before-match detail.</param>
        /// <returns>The RR moved, or null when it cannot be told.</returns>
        public static int? FromBeforeMatch(ValorantRankHistory entry)
        {
            if (entry.TierBeforeId is not { } tierBefore || entry.RrBeforeUpdate is not { } rrBefore)
            {
                return null;
            }

            if (tierBefore < FirstRankedTier || entry.TierId < FirstRankedTier)
            {
                return null;
            }

            if (tierBefore >= FirstImmortalTier || entry.TierId >= FirstImmortalTier)
            {
                return tierBefore == entry.TierId ? entry.RankedRating - rrBefore : null;
            }

            return entry.Elo - (((tierBefore - FirstRankedTier) * 100) + rrBefore);
        }

        /// <summary>
        /// Fills <see cref="ValorantRankHistory.RrDelta"/> on every match entry of one player. Entries carrying
        /// the before-match detail use it; older ones fall back on the Elo of the previous match of the same act,
        /// which is the same value as long as no match is missing in between.
        /// </summary>
        /// <param name="entries">Every rank entry of one player.</param>
        /// <returns>The number of entries whose delta changed.</returns>
        public static int FillAll(IEnumerable<ValorantRankHistory> entries)
        {
            var changed = 0;
            ValorantRankHistory? previous = null;

            foreach (var entry in entries.Where(x => x.MatchId != null).OrderBy(x => x.CreatedOn).ThenBy(x => x.Id))
            {
                var delta = FromBeforeMatch(entry);

                if (delta is null
                    && entry.TierBeforeId is null
                    && previous is not null
                    && previous.SeasonId == entry.SeasonId
                    && previous.TierId is >= FirstRankedTier and < FirstImmortalTier
                    && entry.TierId is >= FirstRankedTier and < FirstImmortalTier)
                {
                    delta = entry.Elo - previous.Elo;
                }

                if (entry.RrDelta != delta)
                {
                    entry.RrDelta = delta;
                    changed++;
                }

                previous = entry;
            }

            return changed;
        }
    }
}
