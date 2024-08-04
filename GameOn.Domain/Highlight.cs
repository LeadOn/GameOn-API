// <copyright file="Highlight.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Highlight class.
    /// </summary>
    public class Highlight
    {
        /// <summary>
        /// Gets or sets Highlight ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Highlight name.
        /// </summary>
        public string Name { get; set; } = "Random name";

        /// <summary>
        /// Gets or sets Highlight description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets Created By ID.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Gets or sets Fifa Game ID.
        /// </summary>
        public int FifaGameId { get; set; }

        /// <summary>
        /// Gets or sets External URL.
        /// </summary>
        public string? ExternalUrl { get; set; }

        /// <summary>
        /// Gets or sets Created by.
        /// </summary>
        [JsonIgnore]
        public virtual Player CreatedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets Game.
        /// </summary>
        [JsonIgnore]
        public virtual FifaGamePlayed FifaGame { get; set; } = null!;
    }
}