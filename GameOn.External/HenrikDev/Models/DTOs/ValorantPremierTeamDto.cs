// <copyright file="ValorantPremierTeamDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Payload of the live Premier team lookup (premier v2).
    /// </summary>
    public class ValorantPremierTeamDto
    {
        /// <summary>
        /// Gets or sets the roster UUID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

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

        /// <summary>
        /// Gets or sets the ISO 8601 UTC creation date.
        /// </summary>
        [JsonProperty("created_at")]
        public string? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the icon and colors.
        /// </summary>
        [JsonProperty("customization")]
        public ValorantPremierCustomizationDto? Customization { get; set; }

        /// <summary>
        /// Gets or sets the members.
        /// </summary>
        [JsonProperty("member")]
        public List<ValorantPremierMemberDto> Members { get; set; } = new List<ValorantPremierMemberDto>();

        /// <summary>
        /// Gets or sets the current season.
        /// </summary>
        [JsonProperty("current_season")]
        public ValorantPremierSeasonDto? CurrentSeason { get; set; }

        /// <summary>
        /// Gets or sets every season played, kept as raw JSON.
        /// </summary>
        [JsonProperty("seasons")]
        public JToken? Seasons { get; set; }
    }
}
