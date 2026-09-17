// <copyright file="LoLCoachQueueStatusDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using System;

    /// <summary>
    /// Where a requested coach analysis stands in the queue. Returned alongside a 202 by both coach routes,
    /// so that waiting reads as a place in a line rather than as an endpoint that hangs.
    /// </summary>
    public class LoLCoachQueueStatusDto
    {
        /// <summary>
        /// Gets or sets the match being waited on.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the GameOn! player being waited on.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the place in the line, 1 being the analysis currently running.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets how many analyses are waiting in total, the running one included.
        /// </summary>
        public int QueueLength { get; set; }

        /// <summary>
        /// Gets or sets the estimated wait until this analysis is readable, in seconds. Derived from how long
        /// the last generations actually took rather than from a constant, so it follows the model's real pace.
        /// </summary>
        public int EstimatedWaitSeconds { get; set; }

        /// <summary>
        /// Gets or sets the instant this analysis was asked for.
        /// </summary>
        public DateTime EnqueuedOn { get; set; }
    }
}
