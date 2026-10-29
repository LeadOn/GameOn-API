// <copyright file="ValorantMatchTeamRoundsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Team round counters inside a HenrikDev v4 match team entry.
    /// </summary>
    public class ValorantMatchTeamRoundsDto
    {
        /// <summary>
        /// Gets or sets the number of rounds won by the team.
        /// </summary>
        [JsonProperty("won")]
        public int Won { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds lost by the team.
        /// </summary>
        [JsonProperty("lost")]
        public int Lost { get; set; }
    }
}
