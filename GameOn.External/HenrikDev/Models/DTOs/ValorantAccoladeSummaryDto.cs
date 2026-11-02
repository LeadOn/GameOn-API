// <copyright file="ValorantAccoladeSummaryDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// All-time and per-act accolade totals.
    /// </summary>
    public class ValorantAccoladeSummaryDto
    {
        /// <summary>
        /// Gets or sets the all-time totals.
        /// </summary>
        [JsonProperty("all_time")]
        public List<ValorantAccoladeTotalDto> AllTime { get; set; } = new List<ValorantAccoladeTotalDto>();

        /// <summary>
        /// Gets or sets the totals of each act.
        /// </summary>
        [JsonProperty("seasons")]
        public List<ValorantAccoladeSeasonDto> Seasons { get; set; } = new List<ValorantAccoladeSeasonDto>();
    }
}
