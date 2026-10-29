// <copyright file="ValorantMmrDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

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
    }
}