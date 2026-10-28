// <copyright file="ValorantMapComboDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Map identifier and display name pair returned by HenrikDev Valorant endpoints.
    /// </summary>
    public class ValorantMapComboDto
    {
        /// <summary>
        /// Gets or sets the map UUID or asset path.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the map display name (e.g. Ascent).
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
