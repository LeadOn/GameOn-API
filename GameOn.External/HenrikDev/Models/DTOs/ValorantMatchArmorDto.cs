// <copyright file="ValorantMatchArmorDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Armor descriptor inside a HenrikDev v4 round economy entry.
    /// </summary>
    public class ValorantMatchArmorDto
    {
        /// <summary>
        /// Gets or sets the armor UUID.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the armor display name (e.g. "Light Armor", "Heavy Armor").
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
