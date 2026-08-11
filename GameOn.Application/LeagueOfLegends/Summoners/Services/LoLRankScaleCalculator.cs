// <copyright file="LoLRankScaleCalculator.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Services
{
    /// <summary>
    /// Places a Riot tier/division/LP reading on a single continuous scale, so two readings can be
    /// subtracted even across a promotion or demotion. Extracted out of <c>GetLoLHomeStatsQueryHandler</c>
    /// (the only place that had it) so <c>GetAllLeaguePlayersQueryHandler</c> can reuse the exact same
    /// scale for the per-player 7-day LP trend, instead of duplicating it a second time. Backend-only
    /// concern: unrelated to the front's <c>lol-tier.util.ts</c>, which only handles tier display (labels,
    /// colors, on-screen sort), not LP deltas.
    /// </summary>
    public static class LoLRankScaleCalculator
    {
        private const int PointsPerDivision = 100;
        private const int DivisionsPerTier = 4;

        // Lowest to highest. Riot stores Tier/Rank as plain uppercase strings straight off league-v4
        // (LeagueEntryDto), no shared enum exists in the codebase to reuse.
        private static readonly string[] TierOrder =
        {
            "IRON", "BRONZE", "SILVER", "GOLD", "PLATINUM", "EMERALD", "DIAMOND", "MASTER", "GRANDMASTER", "CHALLENGER",
        };

        // Lowest to highest. Master and above have no divisions (Riot still sends "I" as a placeholder,
        // never relied upon below).
        private static readonly string[] DivisionOrder = { "IV", "III", "II", "I" };

        private static readonly int MasterTierIndex = Array.IndexOf(TierOrder, "MASTER");

        /// <summary>
        /// Places a tier/division/LP reading on a single continuous scale (100 points per division, 400
        /// per tier below Master), so two readings can be subtracted even across a promotion or demotion.
        /// Master, Grandmaster and Challenger share the same continuous LP scale above that (the boundary
        /// between them is a top-N cut, not a fixed LP threshold), so they're treated as one band.
        /// </summary>
        /// <param name="tier">Tier, as stored by Riot (e.g. "GOLD").</param>
        /// <param name="rank">Division within the tier, as stored by Riot (e.g. "II"). Ignored for Master and above.</param>
        /// <param name="leaguePoints">League points within the tier/division.</param>
        /// <returns>The normalized LP value, or null when the tier/rank isn't recognized (e.g. "UNRANKED").</returns>
        public static int? NormalizedLp(string tier, string rank, int leaguePoints)
        {
            var tierIndex = Array.IndexOf(TierOrder, tier.ToUpperInvariant());

            if (tierIndex < 0)
            {
                return null;
            }

            if (tierIndex >= MasterTierIndex)
            {
                return (MasterTierIndex * DivisionsPerTier * PointsPerDivision) + leaguePoints;
            }

            var divisionIndex = Array.IndexOf(DivisionOrder, rank.ToUpperInvariant());

            if (divisionIndex < 0)
            {
                return null;
            }

            return (tierIndex * DivisionsPerTier * PointsPerDivision) + (divisionIndex * PointsPerDivision) + leaguePoints;
        }
    }
}
