// <copyright file="ValorantMatchKillDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Kill event inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchKillDto
    {
        /// <summary>
        /// Gets or sets the elapsed time within the round in milliseconds.
        /// </summary>
        [JsonProperty("time_in_round_in_ms")]
        public int TimeInRoundInMs { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time within the match in milliseconds.
        /// </summary>
        [JsonProperty("time_in_match_in_ms")]
        public int TimeInMatchInMs { get; set; }

        /// <summary>
        /// Gets or sets the 0-based round index in which the kill occurred.
        /// </summary>
        [JsonProperty("round")]
        public int Round { get; set; }

        /// <summary>
        /// Gets or sets the killer player reference.
        /// </summary>
        [JsonProperty("killer")]
        public ValorantMatchPlayerRefDto? Killer { get; set; }

        /// <summary>
        /// Gets or sets the victim player reference.
        /// </summary>
        [JsonProperty("victim")]
        public ValorantMatchPlayerRefDto? Victim { get; set; }

        /// <summary>
        /// Gets or sets the assisting players on the kill.
        /// </summary>
        [JsonProperty("assistants")]
        public List<ValorantMatchPlayerRefDto> Assistants { get; set; } = new List<ValorantMatchPlayerRefDto>();

        /// <summary>
        /// Gets or sets the weapon or ability used to score the kill.
        /// </summary>
        [JsonProperty("weapon")]
        public ValorantMatchWeaponDto? Weapon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether secondary fire (ADS / right-click) was used.
        /// </summary>
        [JsonProperty("secondary_fire_mode")]
        public bool SecondaryFireMode { get; set; }
    }
}
