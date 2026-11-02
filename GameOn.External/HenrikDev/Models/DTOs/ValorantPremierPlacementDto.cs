// <copyright file="ValorantPremierPlacementDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Placement of a Premier team over a season.
    /// </summary>
    public class ValorantPremierPlacementDto
    {
        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        [JsonProperty("points")]
        public int Points { get; set; }

        /// <summary>
        /// Gets or sets the conference.
        /// </summary>
        [JsonProperty("conference")]
        public string? Conference { get; set; }

        /// <summary>
        /// Gets or sets the division.
        /// </summary>
        [JsonProperty("division")]
        public int Division { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the placement is provisional.
        /// </summary>
        [JsonProperty("is_provisional")]
        public bool IsProvisional { get; set; }
    }
}
