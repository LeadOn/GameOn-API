// <copyright file="ValFunStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System;

    /// <summary>
    /// A single Valorant fun stat award, with its record holder.
    /// </summary>
    public class ValFunStatDto
    {
        /// <summary>
        /// Gets or sets the account holding the record. Identity fields only.
        /// </summary>
        public PlayerDto? Player { get; set; }

        /// <summary>
        /// Gets or sets the record value (deaths, ACS, percentage, ...). Always the value the award is ranked on.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets additional context about the record (agent played, score line, ...).
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// Gets or sets the match in which the record was set, for single-game records. Null for records
        /// built over the whole period.
        /// </summary>
        public string? MatchId { get; set; }

        /// <summary>
        /// Gets or sets the date on which the record was set, if relevant.
        /// </summary>
        public DateTime? GameDate { get; set; }
    }
}
