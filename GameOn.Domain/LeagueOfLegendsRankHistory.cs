// <copyright file="LeagueOfLegendsRankHistory.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    /// <summary>
    /// Player class.
    /// </summary>
    public class LeagueOfLegendsRankHistory
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets player's ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets player's Queue Type.
        /// </summary>
        public string QueueType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Tier.
        /// </summary>
        public string Tier { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Rank.
        /// </summary>
        public string Rank { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets League Points.
        /// </summary>
        public int LeaguePoints { get; set; }

        /// <summary>
        /// Gets or sets Wins.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets Losses.
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a player is on Hot streak or not.
        /// </summary>
        public bool HotStreak { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a player is a veteran or not.
        /// </summary>
        public bool Veteran { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a player is a fresh blood or not.
        /// </summary>
        public bool FreshBlood { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a player is inactive or not.
        /// </summary>
        public bool Inactive { get; set; } = false;

        /// <summary>
        /// Gets or sets player's creation date.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Tournaments won.
        /// </summary>
        public virtual Player Player { get; set; } = null!;
    }
}