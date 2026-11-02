// <copyright file="ValorantMmrHistoryEntryDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Individual competitive match MMR update entry inside <see cref="ValorantMmrHistoryDto"/>.
    /// </summary>
    public class ValorantMmrHistoryEntryDto
    {
        /// <summary>
        /// Gets or sets the competitive match UUID.
        /// </summary>
        [JsonProperty("match_id")]
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the player's tier after the match.
        /// </summary>
        [JsonProperty("tier")]
        public ValorantTierComboDto? Tier { get; set; }

        /// <summary>
        /// Gets or sets the map on which the match was played.
        /// </summary>
        [JsonProperty("map")]
        public ValorantMapComboDto? Map { get; set; }

        /// <summary>
        /// Gets or sets the season/act in which the match was played.
        /// </summary>
        [JsonProperty("season")]
        public ValorantSeasonComboDto? Season { get; set; }

        /// <summary>
        /// Gets or sets the player's Ranked Rating (RR) after the match.
        /// </summary>
        [JsonProperty("rr")]
        public int Rr { get; set; }

        /// <summary>
        /// Gets or sets the RR gained or lost on this match.
        /// </summary>
        [JsonProperty("last_change")]
        public int LastChange { get; set; }

        /// <summary>
        /// Gets or sets the player's continuous cross-tier Elo after the match.
        /// </summary>
        [JsonProperty("elo")]
        public int Elo { get; set; }

        /// <summary>
        /// Gets or sets the RR refunded due to cheater rollback.
        /// </summary>
        [JsonProperty("refunded_rr")]
        public int RefundedRr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a rank protection shield prevented demotion on this match.
        /// </summary>
        [JsonProperty("was_derank_protected")]
        public bool WasDerankProtected { get; set; }

        /// <summary>
        /// Gets or sets the ISO 8601 UTC timestamp of the match.
        /// </summary>
        [JsonProperty("date")]
        public string? Date { get; set; }

        /// <summary>
        /// Gets or sets the queue ID of the match (e.g. competitive). Empty on old stored records.
        /// </summary>
        [JsonProperty("queue_id")]
        public string? QueueId { get; set; }

        /// <summary>
        /// Gets or sets the match length in milliseconds. 0 on old stored records.
        /// </summary>
        [JsonProperty("match_length")]
        public long? MatchLength { get; set; }

        /// <summary>
        /// Gets or sets the tier before the match.
        /// </summary>
        [JsonProperty("tier_before_update")]
        public ValorantTierComboDto? TierBeforeUpdate { get; set; }

        /// <summary>
        /// Gets or sets the RR before the match.
        /// </summary>
        [JsonProperty("rr_before_update")]
        public int? RrBeforeUpdate { get; set; }

        /// <summary>
        /// Gets or sets the RR bonus earned from individual performance.
        /// </summary>
        [JsonProperty("rr_performance_bonus")]
        public int? RrPerformanceBonus { get; set; }

        /// <summary>
        /// Gets or sets the RR penalty factor (e.g. 0.25 for a party size disparity).
        /// </summary>
        [JsonProperty("rr_penalty")]
        public double? RrPenalty { get; set; }

        /// <summary>
        /// Gets or sets the RR penalty applied for being AFK.
        /// </summary>
        [JsonProperty("afk_penalty")]
        public int? AfkPenalty { get; set; }

        /// <summary>
        /// Gets or sets the RR loss forgiven because the match was played on a new map.
        /// </summary>
        [JsonProperty("new_map_incentive_rr_forgiven")]
        public int? NewMapIncentiveRrForgiven { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the match was a placement match.
        /// </summary>
        [JsonProperty("is_placement_match")]
        public bool? IsPlacementMatch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the match replenished the rank protection shields.
        /// </summary>
        [JsonProperty("was_derank_protection_replenished")]
        public bool? WasDerankProtectionReplenished { get; set; }
    }
}