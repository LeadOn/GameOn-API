// <copyright file="LoLGameParticipantRankChange.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGameParticipantRankChange class. League points won or lost by a tracked player in one ranked
    /// game, with the rank reading on each side of it.
    /// </summary>
    /// <remarks>
    /// Riot exposes no per-game LP anywhere in its public API: match-v5 knows nothing about ranks, and
    /// league-v4 only gives the current reading. The value is derived from two consecutive
    /// <see cref="LeagueOfLegendsRankHistory"/> snapshots, and only when exactly one game of that queue
    /// separates them (see <c>LoLGameRankChangeCalculator</c>). A missing row therefore means "unknown",
    /// never "zero": a game played between the same two snapshots as another one has no row at all.
    /// </remarks>
    public class LoLGameParticipantRankChange
    {
        /// <summary>
        /// Gets or sets the LoL Game Participant ID (shared primary key / foreign key).
        /// </summary>
        public int LoLGameParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the league points won (positive) or lost (negative or zero) in this game,
        /// promotions and demotions included (e.g. GOLD II 90 LP to GOLD I 10 LP is +20).
        /// </summary>
        public int LeaguePointsChange { get; set; }

        /// <summary>
        /// Gets or sets the tier before the game (e.g. "GOLD").
        /// </summary>
        public string TierBefore { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the division before the game (e.g. "II").
        /// </summary>
        public string RankBefore { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the league points before the game.
        /// </summary>
        public int LeaguePointsBefore { get; set; }

        /// <summary>
        /// Gets or sets the tier after the game (e.g. "GOLD").
        /// </summary>
        public string TierAfter { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the division after the game (e.g. "I").
        /// </summary>
        public string RankAfter { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the league points after the game.
        /// </summary>
        public int LeaguePointsAfter { get; set; }

        /// <summary>
        /// Gets or sets the date this change was last computed.
        /// </summary>
        public DateTime ComputedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the participant this change belongs to.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGameParticipant Participant { get; set; } = null!;
    }
}
