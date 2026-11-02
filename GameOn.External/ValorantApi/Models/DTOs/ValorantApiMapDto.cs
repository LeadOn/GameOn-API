// <copyright file="ValorantApiMapDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.ValorantApi.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// A map on <c>valorant-api.com</c>: name, minimap transform and callouts.
    /// </summary>
    public class ValorantApiMapDto
    {
        /// <summary>
        /// Gets or sets the map UUID (same as HenrikDev's map ID).
        /// </summary>
        [JsonProperty("uuid")]
        public string Uuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the map name.
        /// </summary>
        [JsonProperty("displayName")]
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the minimap image URL.
        /// </summary>
        [JsonProperty("displayIcon")]
        public string? DisplayIcon { get; set; }

        /// <summary>
        /// Gets or sets the factor turning the game Y coordinate into the minimap X (0 to 1).
        /// </summary>
        [JsonProperty("xMultiplier")]
        public double XMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the factor turning the game X coordinate into the minimap Y (0 to 1).
        /// </summary>
        [JsonProperty("yMultiplier")]
        public double YMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the offset added to the minimap X.
        /// </summary>
        [JsonProperty("xScalarToAdd")]
        public double XScalarToAdd { get; set; }

        /// <summary>
        /// Gets or sets the offset added to the minimap Y.
        /// </summary>
        [JsonProperty("yScalarToAdd")]
        public double YScalarToAdd { get; set; }

        /// <summary>
        /// Gets or sets the named zones of the map, null on maps without any.
        /// </summary>
        [JsonProperty("callouts")]
        public List<ValorantApiCalloutDto>? Callouts { get; set; }
    }
}
