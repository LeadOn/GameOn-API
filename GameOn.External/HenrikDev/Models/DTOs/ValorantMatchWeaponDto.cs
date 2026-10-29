// <copyright file="ValorantMatchWeaponDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Weapon or ability descriptor inside a HenrikDev v4 match kill event.
    /// </summary>
    public class ValorantMatchWeaponDto
    {
        /// <summary>
        /// Gets or sets the weapon or ability UUID.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the weapon display name (e.g. "Vandal", "Phantom", "Melee").
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the damage source type (e.g. "Weapon", "Melee", "Ability", "Bomb").
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}
