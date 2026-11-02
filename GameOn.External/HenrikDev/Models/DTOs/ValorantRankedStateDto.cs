// <copyright file="ValorantRankedStateDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Ranked visibility settings returned by MMR v3.
    /// </summary>
    public class ValorantRankedStateDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether the player hides their act rank badge.
        /// </summary>
        [JsonProperty("is_act_rank_badge_hidden")]
        public bool? IsActRankBadgeHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player is anonymized on the leaderboard.
        /// </summary>
        [JsonProperty("is_leaderboard_anonymized")]
        public bool? IsLeaderboardAnonymized { get; set; }
    }
}
