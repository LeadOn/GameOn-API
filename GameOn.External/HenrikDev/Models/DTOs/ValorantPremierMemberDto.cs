// <copyright file="ValorantPremierMemberDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Member of a Premier team.
    /// </summary>
    public class ValorantPremierMemberDto
    {
        /// <summary>
        /// Gets or sets the member's PUUID.
        /// </summary>
        [JsonProperty("puuid")]
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role (OWNER, MEMBER...).
        /// </summary>
        [JsonProperty("role")]
        public ValorantIdNameDto? Role { get; set; }

        /// <summary>
        /// Gets or sets the ISO 8601 UTC join date.
        /// </summary>
        [JsonProperty("joined_at")]
        public string? JoinedAt { get; set; }
    }
}
