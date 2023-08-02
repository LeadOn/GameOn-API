// <copyright file="TeamPlayer.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Entities
{
    /// <summary>
    /// GamePlayed class.
    /// </summary>
    public class TeamPlayer
    {
        /// <summary>
        /// Gets or sets ID..
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets GamePlayed ID.
        /// </summary>
        public int GamePlayedId { get; set; }

        /// <summary>
        /// Gets or sets Team (team 1 = 0, team 2 = 1).
        /// </summary>
        public int Team { get; set; }

        /// <summary>
        /// Gets or sets Player.
        /// </summary>
        public virtual Player Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets GamePlayed.
        /// </summary>
        public virtual GamePlayed GamePlayed { get; set; } = null!;
    }
}