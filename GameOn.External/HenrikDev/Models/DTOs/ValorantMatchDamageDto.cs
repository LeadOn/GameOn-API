// <copyright file="ValorantMatchDamageDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Damage dealt and received counters inside a HenrikDev v4 match player stats object.
    /// </summary>
    public class ValorantMatchDamageDto
    {
        /// <summary>
        /// Gets or sets the total damage dealt across the match.
        /// </summary>
        [JsonProperty("dealt")]
        public int Dealt { get; set; }

        /// <summary>
        /// Gets or sets the total damage received across the match.
        /// </summary>
        [JsonProperty("received")]
        public int Received { get; set; }
    }
}
