// <copyright file="ValorantApiCalloutDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.ValorantApi.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Named zone of a map (e.g. A Main).
    /// </summary>
    public class ValorantApiCalloutDto
    {
        /// <summary>
        /// Gets or sets the zone name (e.g. Main).
        /// </summary>
        [JsonProperty("regionName")]
        public string? RegionName { get; set; }

        /// <summary>
        /// Gets or sets the area the zone belongs to (A, B, C, Mid, Attacker Side...).
        /// </summary>
        [JsonProperty("superRegionName")]
        public string? SuperRegionName { get; set; }

        /// <summary>
        /// Gets or sets the zone's reference point, in game coordinates.
        /// </summary>
        [JsonProperty("location")]
        public ValorantApiVectorDto? Location { get; set; }
    }
}
