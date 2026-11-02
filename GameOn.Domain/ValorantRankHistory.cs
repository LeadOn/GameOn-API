// <copyright file="ValorantRankHistory.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// ValorantRankHistory class. Stores either a post-match MMR change entry (with <see cref="MatchId"/>)
    /// or a standalone rank snapshot for a player.
    /// </summary>
    public class ValorantRankHistory
    {
        private const string CompetitiveTiersTableUuid = "03621f52-342b-cf4e-4f86-9350a49c6d04";

        /// <summary>
        /// Gets or sets the entry ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the competitive match UUID that produced this rank entry, or null when recorded
        /// from a standalone MMR snapshot.
        /// </summary>
        public string? MatchId { get; set; }

        /// <summary>
        /// Gets or sets the Valorant Act/Season UUID.
        /// </summary>
        public string? SeasonId { get; set; }

        /// <summary>
        /// Gets or sets the Valorant Act/Season short code (e.g. e9a3).
        /// </summary>
        public string? SeasonShort { get; set; }

        /// <summary>
        /// Gets or sets the numeric tier ID (0 = Unranked, 3 = Iron 1 ... 27 = Radiant).
        /// </summary>
        public int TierId { get; set; }

        /// <summary>
        /// Gets or sets the tier display name (e.g. Platinum 3).
        /// </summary>
        public string TierName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Ranked Rating (RR) within the current tier.
        /// </summary>
        public int RankedRating { get; set; }

        /// <summary>
        /// Gets or sets the continuous cross-tier Elo score reported by Valorant.
        /// </summary>
        public int Elo { get; set; }

        /// <summary>
        /// Gets or sets the RR gained or lost on the match that produced this entry.
        /// </summary>
        public int LastChange { get; set; }

        /// <summary>
        /// Gets or sets the RR refunded due to cheater rollback.
        /// </summary>
        public int RefundedRr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a rank protection shield absorbed a demotion at 0 RR.
        /// </summary>
        public bool WasDerankProtected { get; set; }

        /// <summary>
        /// Gets or sets the map identifier/path for the match that produced this entry.
        /// </summary>
        public string? MapId { get; set; }

        /// <summary>
        /// Gets or sets the map display name (e.g. Ascent).
        /// </summary>
        public string? MapName { get; set; }

        /// <summary>
        /// Gets or sets the queue ID of the match (e.g. competitive), null when unknown.
        /// </summary>
        public string? QueueId { get; set; }

        /// <summary>
        /// Gets or sets the match length in milliseconds, null when unknown.
        /// </summary>
        public long? MatchLengthMs { get; set; }

        /// <summary>
        /// Gets or sets the tier ID held before the match.
        /// </summary>
        public int? TierBeforeId { get; set; }

        /// <summary>
        /// Gets or sets the tier name held before the match.
        /// </summary>
        public string? TierBeforeName { get; set; }

        /// <summary>
        /// Gets or sets the RR held before the match.
        /// </summary>
        public int? RrBeforeUpdate { get; set; }

        /// <summary>
        /// Gets or sets the RR bonus earned from individual performance.
        /// </summary>
        public int? RrPerformanceBonus { get; set; }

        /// <summary>
        /// Gets or sets the RR penalty factor applied to the match (e.g. 0.25 for a party size disparity).
        /// </summary>
        public double? RrPenalty { get; set; }

        /// <summary>
        /// Gets or sets the RR penalty applied for being AFK.
        /// </summary>
        public int? AfkPenalty { get; set; }

        /// <summary>
        /// Gets or sets the RR loss forgiven because the match was played on a new map.
        /// </summary>
        public int? NewMapIncentiveRrForgiven { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the match was a placement match.
        /// </summary>
        public bool? IsPlacementMatch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the match replenished the rank protection shields.
        /// </summary>
        public bool? WasDerankProtectionReplenished { get; set; }

        /// <summary>
        /// Gets or sets the RR actually won or lost on the match, computed from the Elo before and after it.
        /// Unlike <see cref="LastChange"/>, which is the change Riot announces (-22 on a loss absorbed at 0 RR,
        /// +16 on a promotion that really gave +22), it adds up across tiers. Null when it cannot be computed
        /// (placement, unranked side, Immortal and above).
        /// </summary>
        public int? RrDelta { get; set; }

        /// <summary>
        /// Gets or sets the UTC date of this rank entry.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets the small rank icon URL on <c>media.valorant-api.com</c>.
        /// </summary>
        [NotMapped]
        public string? SmallIconUrl => this.TierId == 0 || (this.TierId >= 3 && this.TierId <= 27)
            ? $"https://media.valorant-api.com/competitivetiers/{CompetitiveTiersTableUuid}/{this.TierId}/smallicon.png"
            : null;

        /// <summary>
        /// Gets the large rank icon URL on <c>media.valorant-api.com</c>.
        /// </summary>
        [NotMapped]
        public string? LargeIconUrl => this.TierId == 0 || (this.TierId >= 3 && this.TierId <= 27)
            ? $"https://media.valorant-api.com/competitivetiers/{CompetitiveTiersTableUuid}/{this.TierId}/largeicon.png"
            : null;

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="MatchId"/> is a match stored in database, so a
        /// client only links to matches that actually open. The MMR history knows ranked matches that were
        /// never imported. Not persisted: filled by the rank history read only.
        /// </summary>
        [NotMapped]
        public bool MatchStored { get; set; }

        /// <summary>
        /// Gets or sets the player this rank entry belongs to.
        /// </summary>
        [JsonIgnore]
        public virtual Player Player { get; set; } = null!;
    }
}