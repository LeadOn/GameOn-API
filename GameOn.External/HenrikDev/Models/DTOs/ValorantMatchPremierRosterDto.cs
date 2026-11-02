// <copyright file="ValorantMatchPremierRosterDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Premier roster of a team in a Premier match.
    /// </summary>
    public class ValorantMatchPremierRosterDto
    {
        /// <summary>
        /// Gets or sets the roster UUID.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the team name.
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the team tag.
        /// </summary>
        [JsonProperty("tag")]
        public string? Tag { get; set; }
    }
}
