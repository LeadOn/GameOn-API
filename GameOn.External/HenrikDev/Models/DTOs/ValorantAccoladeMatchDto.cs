// <copyright file="ValorantAccoladeMatchDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Accolades of every player of one match.
    /// </summary>
    public class ValorantAccoladeMatchDto
    {
        /// <summary>
        /// Gets or sets the match UUID.
        /// </summary>
        [JsonProperty("match_id")]
        public string? MatchId { get; set; }

        /// <summary>
        /// Gets or sets the ISO 8601 UTC start date.
        /// </summary>
        [JsonProperty("started_at")]
        public string? StartedAt { get; set; }

        /// <summary>
        /// Gets or sets each player's accolades.
        /// </summary>
        [JsonProperty("players")]
        public List<ValorantAccoladeMatchPlayerDto> Players { get; set; } = new List<ValorantAccoladeMatchPlayerDto>();
    }
}
