// <copyright file="ValorantAccoladeSummary.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// How often a player earned one Valorant accolade (end-of-game gold star), all-time or over one act.
    /// </summary>
    public class ValorantAccoladeSummary
    {
        /// <summary>
        /// Gets or sets the primary key ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the act UUID, null for the all-time summary.
        /// </summary>
        public string? SeasonId { get; set; }

        /// <summary>
        /// Gets or sets the act short code, null for the all-time summary.
        /// </summary>
        public string? SeasonShort { get; set; }

        /// <summary>
        /// Gets or sets the accolade UUID.
        /// </summary>
        public string AccoladeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accolade type (e.g. kills, clutches, top_frag).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets how many times the accolade was earned.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the best value reached on a single game.
        /// </summary>
        public double BestValue { get; set; }

        /// <summary>
        /// Gets or sets the UTC date this row was last refreshed.
        /// </summary>
        public DateTime RefreshedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        [JsonIgnore]
        public virtual Player Player { get; set; } = null!;
    }
}
