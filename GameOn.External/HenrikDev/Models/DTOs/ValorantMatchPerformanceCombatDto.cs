// <copyright file="ValorantMatchPerformanceCombatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Combat ratings of Riot's performance rating (double_down, down, neutral, up, double_up).
    /// </summary>
    public class ValorantMatchPerformanceCombatDto
    {
        /// <summary>
        /// Gets or sets the kill impact rating.
        /// </summary>
        [JsonProperty("kill_impact")]
        public string? KillImpact { get; set; }

        /// <summary>
        /// Gets or sets the death impact rating.
        /// </summary>
        [JsonProperty("death_impact")]
        public string? DeathImpact { get; set; }

        /// <summary>
        /// Gets or sets the damage rating.
        /// </summary>
        [JsonProperty("damage")]
        public string? Damage { get; set; }

        /// <summary>
        /// Gets or sets the trades rating.
        /// </summary>
        [JsonProperty("trades")]
        public string? Trades { get; set; }
    }
}
