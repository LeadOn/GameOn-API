// <copyright file="LoLDuoStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using GameOn.Domain;

    /// <summary>
    /// LoLDuoStatDto class. How often a summoner teamed up with another tracked GameOn player, and how well.
    /// </summary>
    public class LoLDuoStatDto
    {
        /// <summary>
        /// Gets or sets the teammate played with.
        /// </summary>
        public Player Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of games played together (same match, same team).
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets the number of games won together.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the win rate of the games played together, as a percentage.
        /// </summary>
        public double WinRate { get; set; }
    }
}
