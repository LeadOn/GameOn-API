// <copyright file="ValorantMatchBehaviorDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Behavioral counters (AFK rounds, spawn rounds, friendly fire) inside a HenrikDev v4 match player entry.
    /// </summary>
    public class ValorantMatchBehaviorDto
    {
        /// <summary>
        /// Gets or sets the number of AFK rounds.
        /// </summary>
        [JsonProperty("afk_rounds")]
        public double? AfkRounds { get; set; }

        /// <summary>
        /// Gets or sets the friendly-fire damage counters.
        /// </summary>
        [JsonProperty("friendly_fire")]
        public ValorantMatchFriendlyFireDto? FriendlyFire { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds spent in spawn.
        /// </summary>
        [JsonProperty("rounds_in_spawn")]
        public double? RoundsInSpawn { get; set; }
    }
}
