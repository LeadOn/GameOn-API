// <copyright file="ValorantMmrPeakDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// All-time peak competitive rating block returned by <c>GET /valorant/v3/by-puuid/mmr/{affinity}/{platform}/{puuid}</c>.
    /// </summary>
    public class ValorantMmrPeakDto
    {
        /// <summary>
        /// Gets or sets the season/act in which the peak rank was achieved.
        /// </summary>
        [JsonProperty("season")]
        public ValorantSeasonComboDto? Season { get; set; }

        /// <summary>
        /// Gets or sets the peak competitive tier.
        /// </summary>
        [JsonProperty("tier")]
        public ValorantTierComboDto? Tier { get; set; }

        /// <summary>
        /// Gets or sets the Ranked Rating (RR) at peak.
        /// </summary>
        [JsonProperty("rr")]
        public int Rr { get; set; }
    }
}
