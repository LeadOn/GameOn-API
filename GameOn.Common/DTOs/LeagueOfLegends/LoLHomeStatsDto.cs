// <copyright file="LoLHomeStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLHomeStatsDto class. Aggregated recap powering the League of Legends v2 home page, one section
    /// per property so new blocks can be added over time without reshaping the existing ones.
    /// </summary>
    public class LoLHomeStatsDto
    {
        /// <summary>
        /// Gets or sets the squad's weekly activity recap.
        /// </summary>
        public LoLWeeklyActivityDto WeeklyActivity { get; set; } = new LoLWeeklyActivityDto();
    }
}
