// <copyright file="Season.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Season class.
    /// </summary>
    public class Season
    {
        /// <summary>
        /// Gets or sets season's ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets season label.
        /// </summary>
        public string Name { get; set; } = "UNKNOWN";

        /// <summary>
        /// Gets or sets FIFA Games played.
        /// </summary>
        [JsonIgnore]
        public virtual List<FifaGamePlayed> FifaGamePlayed { get; set; } = null!;
    }
}