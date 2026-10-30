// <copyright file="ValorantMatchRoundPlayerStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// One player's round inside a HenrikDev v4 match payload (<c>rounds[].stats[]</c>).
    /// </summary>
    public class ValorantMatchRoundPlayerStatsDto
    {
        /// <summary>
        /// Gets or sets the player the entry is about.
        /// </summary>
        [JsonProperty("player")]
        public ValorantMatchPlayerRefDto? Player { get; set; }

        /// <summary>
        /// Gets or sets the damage dealt by the player in the round, one entry per player hit.
        /// </summary>
        [JsonProperty("damage_events")]
        public List<ValorantMatchRoundDamageEventDto> DamageEvents { get; set; } = new List<ValorantMatchRoundDamageEventDto>();

        /// <summary>
        /// Gets or sets the player's combat score, kills and hits in the round.
        /// </summary>
        [JsonProperty("stats")]
        public ValorantMatchRoundPlayerScoreDto? Stats { get; set; }

        /// <summary>
        /// Gets or sets the player's buy in the round.
        /// </summary>
        [JsonProperty("economy")]
        public ValorantMatchRoundEconomyDto? Economy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player was AFK during the round.
        /// </summary>
        [JsonProperty("was_afk")]
        public bool WasAfk { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player stayed in spawn during the round.
        /// </summary>
        [JsonProperty("stayed_in_spawn")]
        public bool StayedInSpawn { get; set; }
    }
}
