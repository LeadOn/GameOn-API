// <copyright file="ValorantPremierRoundsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Rounds won and lost by a Premier team over a season.
    /// </summary>
    public class ValorantPremierRoundsDto
    {
        /// <summary>
        /// Gets or sets the rounds won.
        /// </summary>
        [JsonProperty("won")]
        public int Won { get; set; }

        /// <summary>
        /// Gets or sets the rounds lost.
        /// </summary>
        [JsonProperty("lost")]
        public int Lost { get; set; }
    }
}
