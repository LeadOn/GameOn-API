// <copyright file="ValorantSeasonResult.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// A player's end-of-act Valorant result (rank badge, final tier, record), as reported by MMR v3.
    /// </summary>
    public class ValorantSeasonResult
    {
        /// <summary>
        /// Gets or sets the primary key ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the act UUID.
        /// </summary>
        public string SeasonId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the act short code (e.g. e11a6).
        /// </summary>
        public string? SeasonShort { get; set; }

        /// <summary>
        /// Gets or sets the act rank (badge) tier ID.
        /// </summary>
        public int? ActRankTierId { get; set; }

        /// <summary>
        /// Gets or sets the act rank (badge) tier name.
        /// </summary>
        public string? ActRankTierName { get; set; }

        /// <summary>
        /// Gets or sets the tier ID held at the end of the act.
        /// </summary>
        public int? EndTierId { get; set; }

        /// <summary>
        /// Gets or sets the tier name held at the end of the act.
        /// </summary>
        public string? EndTierName { get; set; }

        /// <summary>
        /// Gets or sets the RR held at the end of the act.
        /// </summary>
        public int EndRr { get; set; }

        /// <summary>
        /// Gets or sets the ranked games played in the act.
        /// </summary>
        public int Games { get; set; }

        /// <summary>
        /// Gets or sets the ranked wins in the act.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the wins including placement matches.
        /// </summary>
        public int WinsWithPlacements { get; set; }

        /// <summary>
        /// Gets or sets the placement games that were still needed.
        /// </summary>
        public int GamesNeededForRating { get; set; }

        /// <summary>
        /// Gets or sets the final leaderboard rank, null when not ranked on it.
        /// </summary>
        public int? LeaderboardRank { get; set; }

        /// <summary>
        /// Gets or sets the ranking schema of the act (base or ascendant).
        /// </summary>
        public string? RankingSchema { get; set; }

        /// <summary>
        /// Gets or sets the tier IDs of the wins that make the act rank badge, as a JSON array.
        /// </summary>
        public string? ActWinsJson { get; set; }

        /// <summary>
        /// Gets or sets the act prestige as raw JSON.
        /// </summary>
        public string? PrestigeJson { get; set; }

        /// <summary>
        /// Gets or sets the UTC date this row was last refreshed.
        /// </summary>
        public DateTime RefreshedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets the large icon URL of the act rank on <c>media.valorant-api.com</c>.
        /// </summary>
        [NotMapped]
        public string? ActRankIconUrl => this.ActRankTierId is >= 3 and <= 27
            ? $"https://media.valorant-api.com/competitivetiers/03621f52-342b-cf4e-4f86-9350a49c6d04/{this.ActRankTierId}/largeicon.png"
            : null;

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        [JsonIgnore]
        public virtual Player Player { get; set; } = null!;
    }
}
