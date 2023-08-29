// <copyright file="FifaPlayerStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.DTOs
{
    /// <summary>
    /// FifaPlayerStatsDto class.
    /// </summary>
    public class FifaPlayerStatsDto
    {
        /// <summary>
        /// Gets or sets statistics per Platform.
        /// </summary>
        public List<PlatformStatsDto>? StatsPerPlatform { get; set; }

        /// <summary>
        /// Gets or sets Most Played Teams.
        /// </summary>
        public List<TopTeamStatDto>? MostPlayedTeams { get; set; }

        /// <summary>
        /// Gets or sets Most Losses Teams.
        /// </summary>
        public List<TopTeamStatDto>? MostLossesTeams { get; set; }

        /// <summary>
        /// Gets or sets Most Wins Teams.
        /// </summary>
        public List<TopTeamStatDto>? MostWinsTeams { get; set; }
    }
}
