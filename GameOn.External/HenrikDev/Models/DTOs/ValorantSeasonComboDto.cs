// <copyright file="ValorantSeasonComboDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Season/Act UUID and short code pair returned by HenrikDev Valorant endpoints.
    /// </summary>
    public class ValorantSeasonComboDto
    {
        /// <summary>
        /// Gets or sets the season/act UUID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the short season/act code (e.g. e9a3).
        /// </summary>
        [JsonProperty("short")]
        public string Short { get; set; } = string.Empty;
    }
}
