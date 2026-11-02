// <copyright file="ValorantMatchPerformanceDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Riot's performance rating of a player in a match, null on modes Riot does not rate.
    /// </summary>
    public class ValorantMatchPerformanceDto
    {
        /// <summary>
        /// Gets or sets the overall performance score.
        /// </summary>
        [JsonProperty("score")]
        public double? Score { get; set; }

        /// <summary>
        /// Gets or sets the per-round values behind the score.
        /// </summary>
        [JsonProperty("breakdown")]
        public ValorantMatchPerformanceBreakdownDto? Breakdown { get; set; }

        /// <summary>
        /// Gets or sets the ratings.
        /// </summary>
        [JsonProperty("ratings")]
        public ValorantMatchPerformanceRatingsDto? Ratings { get; set; }
    }
}
