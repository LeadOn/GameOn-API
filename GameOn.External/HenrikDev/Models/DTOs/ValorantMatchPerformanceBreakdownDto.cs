// <copyright file="ValorantMatchPerformanceBreakdownDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Per-round values behind Riot's performance rating of a player.
    /// </summary>
    public class ValorantMatchPerformanceBreakdownDto
    {
        /// <summary>
        /// Gets or sets the adjusted kills per round.
        /// </summary>
        [JsonProperty("adjusted_kills")]
        public double? AdjustedKills { get; set; }

        /// <summary>
        /// Gets or sets the adjusted deaths per round.
        /// </summary>
        [JsonProperty("adjusted_deaths")]
        public double? AdjustedDeaths { get; set; }

        /// <summary>
        /// Gets or sets the assists per round.
        /// </summary>
        [JsonProperty("assists")]
        public double? Assists { get; set; }

        /// <summary>
        /// Gets or sets the damage per round.
        /// </summary>
        [JsonProperty("damage")]
        public double? Damage { get; set; }

        /// <summary>
        /// Gets or sets the trades per round.
        /// </summary>
        [JsonProperty("trades")]
        public double? Trades { get; set; }

        /// <summary>
        /// Gets or sets the utility usage per round.
        /// </summary>
        [JsonProperty("utility_usage")]
        public double? UtilityUsage { get; set; }

        /// <summary>
        /// Gets or sets the plants per round.
        /// </summary>
        [JsonProperty("plants")]
        public double? Plants { get; set; }

        /// <summary>
        /// Gets or sets the defuses per round.
        /// </summary>
        [JsonProperty("defuses")]
        public double? Defuses { get; set; }
    }
}
