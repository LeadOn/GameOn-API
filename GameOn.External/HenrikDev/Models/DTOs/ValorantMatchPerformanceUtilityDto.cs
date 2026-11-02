// <copyright file="ValorantMatchPerformanceUtilityDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Utility ratings of Riot's performance rating (double_down, down, neutral, up, double_up).
    /// </summary>
    public class ValorantMatchPerformanceUtilityDto
    {
        /// <summary>
        /// Gets or sets the assists rating.
        /// </summary>
        [JsonProperty("assists")]
        public string? Assists { get; set; }

        /// <summary>
        /// Gets or sets the plants rating.
        /// </summary>
        [JsonProperty("plants")]
        public string? Plants { get; set; }

        /// <summary>
        /// Gets or sets the defuses rating.
        /// </summary>
        [JsonProperty("defuses")]
        public string? Defuses { get; set; }

        /// <summary>
        /// Gets or sets the utility usage rating.
        /// </summary>
        [JsonProperty("utility_usage")]
        public string? UtilityUsage { get; set; }
    }
}
