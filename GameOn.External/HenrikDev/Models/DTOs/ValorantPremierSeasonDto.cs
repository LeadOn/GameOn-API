// <copyright file="ValorantPremierSeasonDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// A Premier team's season.
    /// </summary>
    public class ValorantPremierSeasonDto
    {
        /// <summary>
        /// Gets or sets the season UUID.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the season name.
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the team is enrolled.
        /// </summary>
        [JsonProperty("enrolled")]
        public bool Enrolled { get; set; }

        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        [JsonProperty("stats")]
        public ValorantPremierStatsDto? Stats { get; set; }

        /// <summary>
        /// Gets or sets the placement.
        /// </summary>
        [JsonProperty("placement")]
        public ValorantPremierPlacementDto? Placement { get; set; }
    }
}
