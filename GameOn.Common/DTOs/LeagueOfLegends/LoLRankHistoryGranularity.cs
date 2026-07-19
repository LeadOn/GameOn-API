// <copyright file="LoLRankHistoryGranularity.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLRankHistoryGranularity enum. Buckets rank history snapshots down to one per period.
    /// </summary>
    public enum LoLRankHistoryGranularity
    {
        /// <summary>
        /// Keep the last snapshot of each day.
        /// </summary>
        Day,

        /// <summary>
        /// Keep the last snapshot of each ISO week.
        /// </summary>
        Week,

        /// <summary>
        /// Keep the last snapshot of each month.
        /// </summary>
        Month,
    }
}
