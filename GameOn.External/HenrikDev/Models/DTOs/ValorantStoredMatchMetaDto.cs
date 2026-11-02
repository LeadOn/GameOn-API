// <copyright file="ValorantStoredMatchMetaDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Metadata of a stored match (stored-matches v1).
    /// </summary>
    public class ValorantStoredMatchMetaDto
    {
        /// <summary>
        /// Gets or sets the match UUID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        [JsonProperty("map")]
        public ValorantMapComboDto? Map { get; set; }

        /// <summary>
        /// Gets or sets the mode name (e.g. Competitive).
        /// </summary>
        [JsonProperty("mode")]
        public string? Mode { get; set; }

        /// <summary>
        /// Gets or sets the ISO 8601 UTC start date.
        /// </summary>
        [JsonProperty("started_at")]
        public string? StartedAt { get; set; }

        /// <summary>
        /// Gets or sets the act.
        /// </summary>
        [JsonProperty("season")]
        public ValorantSeasonComboDto? Season { get; set; }
    }
}
