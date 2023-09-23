// <copyright file="AdminDashboardDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Common.DTOs
{
    using YuGames.Domain;

    /// <summary>
    /// AdminDashboardDto class.
    /// </summary>
    public class AdminDashboardDto
    {
        /// <summary>
        /// Gets or sets Player.
        /// </summary>
        public Player CurrentUser { get; set; } = new Player();

        /// <summary>
        /// Gets or sets number of platforms.
        /// </summary>
        public int Platforms { get; set; }

        /// <summary>
        /// Gets or sets number of players.
        /// </summary>
        public int Players { get; set; }

        /// <summary>
        /// Gets or sets number of highlights.
        /// </summary>
        public int Highlights { get; set; }

        /// <summary>
        /// Gets or sets number of FifaGames.
        /// </summary>
        public int FifaGames { get; set; }

        /// <summary>
        /// Gets or sets number of Tournaments.
        /// </summary>
        public int Tournaments { get; set; }

        /// <summary>
        /// Gets or sets number of Seasons.
        /// </summary>
        public int Seasons { get; set; }
    }
}