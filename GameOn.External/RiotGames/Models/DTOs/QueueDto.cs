// <copyright file="QueueDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// QueueDto class.
    /// </summary>
    public class QueueDto
    {
        /// <summary>
        /// Gets or sets Queue ID.
        /// </summary>
        [JsonProperty("queueId")]
        public int QueueId { get; set; }

        /// <summary>
        /// Gets or sets Map name.
        /// </summary>
        [JsonProperty("map")]
        public string Map { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets Notes.
        /// </summary>
        [JsonProperty("notes")]
        public string? Notes { get; set; }
    }
}
