// <copyright file="ValorantMatchMetadataDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Match metadata object from HenrikDev Valorant v4 matches endpoint.
    /// </summary>
    public class ValorantMatchMetadataDto
    {
        /// <summary>
        /// Gets or sets the match UUID.
        /// </summary>
        [JsonProperty("match_id")]
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the map descriptor (ID and name).
        /// </summary>
        [JsonProperty("map")]
        public ValorantMapComboDto? Map { get; set; }

        /// <summary>
        /// Gets or sets the Valorant game version string.
        /// </summary>
        [JsonProperty("game_version")]
        public string? GameVersion { get; set; }

        /// <summary>
        /// Gets or sets the match duration in milliseconds.
        /// </summary>
        [JsonProperty("game_length_in_ms")]
        public long GameLengthInMs { get; set; }

        /// <summary>
        /// Gets or sets the ISO-8601 UTC start timestamp.
        /// </summary>
        [JsonProperty("started_at")]
        public string? StartedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the match finished normally.
        /// </summary>
        [JsonProperty("is_completed")]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the queue information.
        /// </summary>
        [JsonProperty("queue")]
        public ValorantMatchQueueDto? Queue { get; set; }

        /// <summary>
        /// Gets or sets the season/act descriptor.
        /// </summary>
        [JsonProperty("season")]
        public ValorantSeasonComboDto? Season { get; set; }

        /// <summary>
        /// Gets or sets the platform ("pc" or "console").
        /// </summary>
        [JsonProperty("platform")]
        public string? Platform { get; set; }

        /// <summary>
        /// Gets or sets the shard/region (e.g. "eu").
        /// </summary>
        [JsonProperty("region")]
        public string? Region { get; set; }

        /// <summary>
        /// Gets or sets the server cluster (e.g. "Paris", "Frankfurt", "London").
        /// </summary>
        [JsonProperty("cluster")]
        public string? Cluster { get; set; }

        /// <summary>
        /// Gets or sets Riot's native MVP reference for the match, if any.
        /// </summary>
        [JsonProperty("mvp")]
        public ValorantMatchPlayerRefDto? Mvp { get; set; }
    }
}
