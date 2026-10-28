// <copyright file="ValorantMmrHistoryDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Payload returned by <c>GET /valorant/v2/by-puuid/mmr-history/{affinity}/{platform}/{puuid}</c>.
    /// </summary>
    public class ValorantMmrHistoryDto
    {
        /// <summary>
        /// Gets or sets the chronological list of competitive MMR updates (newest first).
        /// </summary>
        [JsonProperty("history")]
        public List<ValorantMmrHistoryEntryDto> History { get; set; } = new List<ValorantMmrHistoryEntryDto>();
    }
}
