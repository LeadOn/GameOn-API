// <copyright file="ValorantAccoladeMatchPlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Accolades of one player of a match.
    /// </summary>
    public class ValorantAccoladeMatchPlayerDto
    {
        /// <summary>
        /// Gets or sets the player's PUUID.
        /// </summary>
        [JsonProperty("puuid")]
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accolades earned.
        /// </summary>
        [JsonProperty("accolades")]
        public List<ValorantAccoladeDto> Accolades { get; set; } = new List<ValorantAccoladeDto>();
    }
}
