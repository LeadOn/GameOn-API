// <copyright file="ValorantMatchAbilityCastsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Ability cast counters for a player inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchAbilityCastsDto
    {
        /// <summary>
        /// Gets or sets the C ability (grenade slot) cast count.
        /// </summary>
        [JsonProperty("grenade")]
        public int? Grenade { get; set; }

        /// <summary>
        /// Gets or sets the Q ability (ability1 slot) cast count.
        /// </summary>
        [JsonProperty("ability1")]
        public int? Ability1 { get; set; }

        /// <summary>
        /// Gets or sets the E ability (ability2 / signature slot) cast count.
        /// </summary>
        [JsonProperty("ability2")]
        public int? Ability2 { get; set; }

        /// <summary>
        /// Gets or sets the X ability (ultimate slot) cast count.
        /// </summary>
        [JsonProperty("ultimate")]
        public int? Ultimate { get; set; }
    }
}
