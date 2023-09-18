// <copyright file="TournamentPlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.DTOs
{
    using YuGames.Entities;

    /// <summary>
    /// TournamentPlayerDto class.
    /// </summary>
    public class TournamentPlayerDto
    {
        /// <summary>
        /// Gets or sets Player.
        /// </summary>
        public Player Player { get; set; } = new Player();

        /// <summary>
        /// Gets or sets FIFA Team.
        /// </summary>
        public FifaTeam Team { get; set; } = new FifaTeam();

        /// <summary>
        /// Gets or sets Joined At.
        /// </summary>
        public DateTime JoinedAt { get; set; }

        /// <summary>
        /// Gets or sets Score.
        /// </summary>
        public int Score { get; set; }
    }
}