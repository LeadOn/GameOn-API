// <copyright file="ValorantAccoladesDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Payload of accolades v1: the recent ranked matches with every player's accolades, and the player's totals.
    /// </summary>
    public class ValorantAccoladesDto
    {
        /// <summary>
        /// Gets or sets the recent matches (11 to 20 ranked games).
        /// </summary>
        [JsonProperty("matches")]
        public List<ValorantAccoladeMatchDto> Matches { get; set; } = new List<ValorantAccoladeMatchDto>();

        /// <summary>
        /// Gets or sets the player's totals.
        /// </summary>
        [JsonProperty("summary")]
        public ValorantAccoladeSummaryDto? Summary { get; set; }
    }
}
