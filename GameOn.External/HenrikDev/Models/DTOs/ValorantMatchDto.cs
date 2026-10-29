// <copyright file="ValorantMatchDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Full Valorant match payload from HenrikDev v4 endpoints (<c>/valorant/v4/by-puuid/matches</c> and <c>/valorant/v4/match</c>).
    /// </summary>
    public class ValorantMatchDto
    {
        /// <summary>
        /// Gets or sets the match metadata.
        /// </summary>
        [JsonProperty("metadata")]
        public ValorantMatchMetadataDto? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the participants in the match.
        /// </summary>
        [JsonProperty("players")]
        public List<ValorantMatchPlayerDto> Players { get; set; } = new List<ValorantMatchPlayerDto>();

        /// <summary>
        /// Gets or sets the teams in the match.
        /// </summary>
        [JsonProperty("teams")]
        public List<ValorantMatchTeamDto> Teams { get; set; } = new List<ValorantMatchTeamDto>();

        /// <summary>
        /// Gets or sets the round-by-round breakdown.
        /// </summary>
        [JsonProperty("rounds")]
        public List<ValorantMatchRoundDto> Rounds { get; set; } = new List<ValorantMatchRoundDto>();

        /// <summary>
        /// Gets or sets the chronological kill events across the match.
        /// </summary>
        [JsonProperty("kills")]
        public List<ValorantMatchKillDto> Kills { get; set; } = new List<ValorantMatchKillDto>();
    }
}
