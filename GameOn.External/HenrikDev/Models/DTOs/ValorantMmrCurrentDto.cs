// <copyright file="ValorantMmrCurrentDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Current competitive rating block returned by <c>GET /valorant/v3/by-puuid/mmr/{affinity}/{platform}/{puuid}</c>.
    /// </summary>
    public class ValorantMmrCurrentDto
    {
        /// <summary>
        /// Gets or sets the player's current competitive tier.
        /// </summary>
        [JsonProperty("tier")]
        public ValorantTierComboDto? Tier { get; set; }

        /// <summary>
        /// Gets or sets the player's current Ranked Rating (RR) within the tier.
        /// </summary>
        [JsonProperty("rr")]
        public int Rr { get; set; }

        /// <summary>
        /// Gets or sets the RR change from the most recent competitive match.
        /// </summary>
        [JsonProperty("last_change")]
        public int LastChange { get; set; }

        /// <summary>
        /// Gets or sets the continuous cross-tier Elo score.
        /// </summary>
        [JsonProperty("elo")]
        public int Elo { get; set; }

        /// <summary>
        /// Gets or sets the number of placement matches still required to receive a rating.
        /// </summary>
        [JsonProperty("games_needed_for_rating")]
        public int GamesNeededForRating { get; set; }

        /// <summary>
        /// Gets or sets the remaining rank protection shields at tier floor (0 RR).
        /// </summary>
        [JsonProperty("rank_protection_shields")]
        public int RankProtectionShields { get; set; }
    }
}