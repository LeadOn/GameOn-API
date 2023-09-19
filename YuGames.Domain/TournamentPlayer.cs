// <copyright file="TournamentPlayer.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Domain
{
    /// <summary>
    /// TournamentPlayer class.
    /// </summary>
    public class TournamentPlayer
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets FIFA Team Id.
        /// </summary>
        public int FifaTeamId { get; set; }

        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets Join date.
        /// </summary>
        public DateTime JoinedAt { get; set; }

        /// <summary>
        /// Gets or sets Player.
        /// </summary>
        public virtual Player Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets Tournament.
        /// </summary>
        public virtual Tournament Tournament { get; set; } = null!;

        /// <summary>
        /// Gets or sets GamePlayed.
        /// </summary>
        public virtual FifaTeam FifaTeam { get; set; } = null!;
    }
}