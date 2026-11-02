// <copyright file="ValorantMmrDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Payload returned by <c>GET /valorant/v3/by-puuid/mmr/{affinity}/{platform}/{puuid}</c>.
    /// </summary>
    public class ValorantMmrDto
    {
        /// <summary>
        /// Gets or sets the player's current competitive standing.
        /// </summary>
        [JsonProperty("current")]
        public ValorantMmrCurrentDto? Current { get; set; }

        /// <summary>
        /// Gets or sets the player's all-time peak competitive standing, or null if never ranked.
        /// </summary>
        [JsonProperty("peak")]
        public ValorantMmrPeakDto? Peak { get; set; }

        /// <summary>
        /// Gets or sets the end-of-act results, one entry per act the player was ranked in.
        /// </summary>
        [JsonProperty("seasonal")]
        public List<ValorantMmrSeasonalDto> Seasonal { get; set; } = new List<ValorantMmrSeasonalDto>();

        /// <summary>
        /// Gets or sets the player's ranked visibility settings.
        /// </summary>
        [JsonProperty("ranked_state")]
        public ValorantRankedStateDto? RankedState { get; set; }

        /// <summary>
        /// Gets or sets the lifetime prestige, kept as raw JSON (e.g. <c>{"DIAMOND": {"count": 6}}</c>).
        /// </summary>
        [JsonProperty("lifetime_prestige")]
        public JToken? LifetimePrestige { get; set; }
    }
}