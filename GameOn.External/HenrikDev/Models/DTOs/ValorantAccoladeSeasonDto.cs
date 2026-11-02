// <copyright file="ValorantAccoladeSeasonDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Accolade totals of one act.
    /// </summary>
    public class ValorantAccoladeSeasonDto
    {
        /// <summary>
        /// Gets or sets the act.
        /// </summary>
        [JsonProperty("season")]
        public ValorantSeasonComboDto? Season { get; set; }

        /// <summary>
        /// Gets or sets the totals.
        /// </summary>
        [JsonProperty("accolades")]
        public List<ValorantAccoladeTotalDto> Accolades { get; set; } = new List<ValorantAccoladeTotalDto>();
    }
}
