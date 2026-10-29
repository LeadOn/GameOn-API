// <copyright file="ValorantMatchQueueDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Queue metadata inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchQueueDto
    {
        /// <summary>
        /// Gets or sets the queue identifier (e.g. "competitive", "unrated", "swiftplay", or empty for custom games).
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the human-readable queue name (e.g. "Competitive", "Unrated", "Custom Game").
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the queue mode type (e.g. "Standard", "Skirmish", "Deathmatch").
        /// </summary>
        [JsonProperty("mode_type")]
        public string? ModeType { get; set; }
    }
}
