// <copyright file="ValorantAccoladeTotalDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// How often one accolade was earned over a period.
    /// </summary>
    public class ValorantAccoladeTotalDto
    {
        /// <summary>
        /// Gets or sets the accolade UUID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accolade type.
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets how many times it was earned.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the best value reached on one game.
        /// </summary>
        [JsonProperty("best_value")]
        public double BestValue { get; set; }
    }
}
