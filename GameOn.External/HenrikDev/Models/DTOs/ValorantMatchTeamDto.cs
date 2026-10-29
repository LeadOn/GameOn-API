// <copyright file="ValorantMatchTeamDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Team summary inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchTeamDto
    {
        /// <summary>
        /// Gets or sets the team identifier ("Red", "Blue", or player PUUID in Deathmatch).
        /// </summary>
        [JsonProperty("team_id")]
        public string TeamId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the team placement (1 = winner).
        /// </summary>
        [JsonProperty("placement")]
        public int? Placement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the team won the match.
        /// </summary>
        [JsonProperty("won")]
        public bool Won { get; set; }

        /// <summary>
        /// Gets or sets the rounds won and lost by the team.
        /// </summary>
        [JsonProperty("rounds")]
        public ValorantMatchTeamRoundsDto? Rounds { get; set; }
    }
}
