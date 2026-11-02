// <copyright file="ValorantLeaderboardPlacementDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Leaderboard placement returned by HenrikDev MMR endpoints.
    /// </summary>
    public class ValorantLeaderboardPlacementDto
    {
        /// <summary>
        /// Gets or sets the leaderboard rank.
        /// </summary>
        [JsonProperty("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the ISO 8601 UTC date the placement was last updated.
        /// </summary>
        [JsonProperty("updated_at")]
        public string? UpdatedAt { get; set; }
    }
}
