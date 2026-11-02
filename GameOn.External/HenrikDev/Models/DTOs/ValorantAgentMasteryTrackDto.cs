// <copyright file="ValorantAgentMasteryTrackDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Mastery track of an agent.
    /// </summary>
    public class ValorantAgentMasteryTrackDto
    {
        /// <summary>
        /// Gets or sets the track UUID.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the track name (always null so far).
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the level reached.
        /// </summary>
        [JsonProperty("level")]
        public int? Level { get; set; }
    }
}
