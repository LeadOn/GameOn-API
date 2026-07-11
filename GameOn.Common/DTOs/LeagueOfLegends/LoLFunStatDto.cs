// <copyright file="LoLFunStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using GameOn.Domain;

    /// <summary>
    /// LoLFunStatDto class. Represents a single fun stat award, with its record holder.
    /// </summary>
    public class LoLFunStatDto
    {
        /// <summary>
        /// Gets or sets the player holding the record. Null for awards not tied to a player (ex: cursed patch).
        /// </summary>
        public Player? Player { get; set; }

        /// <summary>
        /// Gets or sets the record value (deaths, pings, gold, percentage, ...).
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets additional context about the record (champion played, score line, patch, ...).
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// Gets or sets the match in which the record was set, if relevant.
        /// </summary>
        public string? MatchId { get; set; }

        /// <summary>
        /// Gets or sets the date on which the record was set, if relevant.
        /// </summary>
        public DateTime? GameDate { get; set; }
    }
}
