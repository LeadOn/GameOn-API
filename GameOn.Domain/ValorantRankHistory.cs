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
        /// Gets or sets the player this rank entry belongs to.
        /// </summary>
        [JsonIgnore]
        public virtual Player Player { get; set; } = null!;
    }
}