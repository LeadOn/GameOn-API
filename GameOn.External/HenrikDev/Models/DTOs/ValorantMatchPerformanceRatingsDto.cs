// <copyright file="ValorantMatchPerformanceRatingsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Ratings of Riot's performance rating.
    /// </summary>
    public class ValorantMatchPerformanceRatingsDto
    {
        /// <summary>
        /// Gets or sets the overall grade (e.g. pass).
        /// </summary>
        [JsonProperty("grade")]
        public string? Grade { get; set; }

        /// <summary>
        /// Gets or sets the combat ratings.
        /// </summary>
        [JsonProperty("combat")]
        public ValorantMatchPerformanceCombatDto? Combat { get; set; }

        /// <summary>
        /// Gets or sets the utility ratings.
        /// </summary>
        [JsonProperty("utility")]
        public ValorantMatchPerformanceUtilityDto? Utility { get; set; }
    }
}
