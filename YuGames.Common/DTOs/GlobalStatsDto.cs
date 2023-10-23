// <copyright file="GlobalStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Common.DTOs
{
    using YuGames.Domain;

    /// <summary>
    /// GlobalStatsDto class.
    /// </summary>
    public class GlobalStatsDto
    {
        /// <summary>
        /// Gets or sets Number of Games played.
        /// </summary>
        public int NumberOfGames { get; set; }

        /// <summary>
        /// Gets or sets best player.
        /// </summary>
        public Player? BestPlayer { get; set; }

        /// <summary>
        /// Gets or sets best player stats.
        /// </summary>
        public FifaPlayerStatsDto? BestPlayerStats { get; set; }

        /// <summary>
        /// Gets or sets worst player.
        /// </summary>
        public Player? WorstPlayer { get; set; }

        /// <summary>
        /// Gets or sets best player stats.
        /// </summary>
        public FifaPlayerStatsDto? WorstPlayerStats { get; set; }
    }
}
