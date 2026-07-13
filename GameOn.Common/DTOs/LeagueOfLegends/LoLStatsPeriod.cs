// <copyright file="LoLStatsPeriod.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLStatsPeriod enum. Restricts stats to a rolling time window.
    /// </summary>
    public enum LoLStatsPeriod
    {
        /// <summary>
        /// No time restriction.
        /// </summary>
        AllTime,

        /// <summary>
        /// Last 7 days.
        /// </summary>
        Week,

        /// <summary>
        /// Last month.
        /// </summary>
        Month,

        /// <summary>
        /// Last 3 months.
        /// </summary>
        ThreeMonths,

        /// <summary>
        /// Last 6 months.
        /// </summary>
        SixMonths,
    }
}
