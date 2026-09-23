// <copyright file="LoLGameRankChangeDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using GameOn.Domain;

    /// <summary>
    /// One ranked game of a player, with the LP it won or lost.
    /// </summary>
    public class LoLGameRankChangeDto
    {
        /// <summary>
        /// Gets or sets the match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the match-v5 queue ID (420 Solo/Duo, 440 Flex).
        /// </summary>
        public int QueueId { get; set; }

        /// <summary>
        /// Gets or sets the game start date.
        /// </summary>
        public DateTime GameStart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player won the game.
        /// </summary>
        public bool Win { get; set; }

        /// <summary>
        /// Gets or sets the champion played.
        /// </summary>
        public string ChampionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the player's kills.
        /// </summary>
        public int Kills { get; set; }

        /// <summary>
        /// Gets or sets the player's deaths.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Gets or sets the player's assists.
        /// </summary>
        public int Assists { get; set; }

        /// <summary>
        /// Gets or sets the LP won or lost, with the rank on each side of the game. Same object as
        /// <c>rankChange</c> on the match endpoints, and null in the same cases: unknown, never zero.
        /// </summary>
        public LoLGameParticipantRankChange? RankChange { get; set; }
    }
}
