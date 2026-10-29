// <copyright file="ValHomeStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using GameOn.Common.DTOs.LeagueOfLegends;

    /// <summary>
    /// Aggregated recap powering the Valorant home page, one section per property, like its League of Legends
    /// counterpart.
    /// </summary>
    public class ValHomeStatsDto
    {
        /// <summary>
        /// Gets or sets the crew's competitive activity on the window picked by <see cref="LoLHomeWindow"/>.
        /// </summary>
        public ValWeeklyActivityDto WeeklyActivity { get; set; } = new ValWeeklyActivityDto();

        /// <summary>
        /// Gets or sets the account with the best net RR gain over the same window as
        /// <see cref="WeeklyActivity"/>. Null when no account has a ranked match recorded in the window.
        /// </summary>
        public ValFactOfTheWeekDto? FactOfTheWeek { get; set; }

        /// <summary>
        /// Gets or sets the crew's fun stat awards over the last rolling month, competitive only (same
        /// computation as <c>GET val/stats/global?rankedOnly=true&amp;period=Month</c>). Unaffected by
        /// <see cref="LoLHomeWindow"/>.
        /// </summary>
        public ValGlobalStatsDto CrewRecords { get; set; } = new ValGlobalStatsDto();
    }
}
