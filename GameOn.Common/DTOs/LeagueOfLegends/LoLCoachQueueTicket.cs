// <copyright file="LoLCoachQueueTicket.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using System;

    /// <summary>
    /// One analysis waiting to be written. Held in memory only: the queue is process-local and a redeployment
    /// simply loses it, which is the accepted trade for keeping the report table a pure cache with no status
    /// column and no migration.
    /// </summary>
    public class LoLCoachQueueTicket
    {
        /// <summary>
        /// Gets or sets the match to analyse.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the GameOn! player to analyse.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an existing report should be thrown away and written again.
        /// </summary>
        public bool ForceRegenerate { get; set; }

        /// <summary>
        /// Gets or sets how many times the provider has already refused this one.
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// Gets or sets the instant this analysis was asked for.
        /// </summary>
        public DateTime EnqueuedOn { get; set; } = DateTime.UtcNow;
    }
}
