// <copyright file="ValorantMatchSpikeEventDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Spike plant or defuse event inside a HenrikDev v4 match round entry.
    /// </summary>
    public class ValorantMatchSpikeEventDto
    {
        /// <summary>
        /// Gets or sets the timestamp within the round in milliseconds.
        /// </summary>
        [JsonProperty("round_time_in_ms")]
        public int? RoundTimeInMs { get; set; }

        /// <summary>
        /// Gets or sets the plant site ("A", "B", "C"), if applicable.
        /// </summary>
        [JsonProperty("site")]
        public string? Site { get; set; }

        /// <summary>
        /// Gets or sets the player who planted or defused the spike.
        /// </summary>
        [JsonProperty("player")]
        public ValorantMatchPlayerRefDto? Player { get; set; }
    }
}
