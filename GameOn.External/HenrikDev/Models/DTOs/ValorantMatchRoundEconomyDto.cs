// <copyright file="ValorantMatchRoundEconomyDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Player's buy for one round, inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchRoundEconomyDto
    {
        /// <summary>
        /// Gets or sets the value of the player's loadout at the start of the round, in credits.
        /// </summary>
        [JsonProperty("loadout_value")]
        public int LoadoutValue { get; set; }

        /// <summary>
        /// Gets or sets the credits left after the buy phase.
        /// </summary>
        [JsonProperty("remaining")]
        public int Remaining { get; set; }

        /// <summary>
        /// Gets or sets the main weapon carried, if any.
        /// </summary>
        [JsonProperty("weapon")]
        public ValorantMatchWeaponDto? Weapon { get; set; }

        /// <summary>
        /// Gets or sets the armor carried, if any.
        /// </summary>
        [JsonProperty("armor")]
        public ValorantMatchArmorDto? Armor { get; set; }
    }
}
