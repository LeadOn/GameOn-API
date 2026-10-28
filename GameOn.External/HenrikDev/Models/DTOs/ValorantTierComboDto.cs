// <copyright file="ValorantTierComboDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Tier ID and name pair returned by HenrikDev Valorant endpoints.
    /// </summary>
    public class ValorantTierComboDto
    {
        /// <summary>
        /// Gets or sets the numeric tier ID (0 = Unranked, 3 = Iron 1 ... 27 = Radiant).
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the tier display name (e.g. Platinum 3).
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
