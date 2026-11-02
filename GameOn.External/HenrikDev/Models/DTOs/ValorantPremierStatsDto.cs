// <copyright file="ValorantPremierStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Record of a Premier team over a season.
    /// </summary>
    public class ValorantPremierStatsDto
    {
        /// <summary>
        /// Gets or sets the wins.
        /// </summary>
        [JsonProperty("wins")]
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the losses.
        /// </summary>
        [JsonProperty("losses")]
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets the matches played.
        /// </summary>
        [JsonProperty("matches")]
        public int Matches { get; set; }

        /// <summary>
        /// Gets or sets the rounds won and lost.
        /// </summary>
        [JsonProperty("rounds")]
        public ValorantPremierRoundsDto? Rounds { get; set; }
    }
}
