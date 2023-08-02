// <copyright file="Platform.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Entities
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Platform class.
    /// </summary>
    public class Platform
    {
        /// <summary>
        /// Gets or sets platform ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Platform name.
        /// </summary>
        public string Name { get; set; } = "Unknown";

        /// <summary>
        /// Gets or sets Games Played on that platform.
        /// </summary>
        [JsonIgnore]
        public virtual List<GamePlayed> GamesPlayed { get; set; } = null!;
    }
}