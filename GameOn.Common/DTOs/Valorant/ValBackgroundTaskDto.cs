// <copyright file="ValBackgroundTaskDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System;

    /// <summary>
    /// A Valorant background task, waiting or running.
    /// </summary>
    public class ValBackgroundTaskDto
    {
        /// <summary>
        /// Gets or sets what the task does.
        /// </summary>
        public ValBackgroundTaskKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the player it covers, null for every match in database (reimport only).
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the oldest match start it covers (reimport only), null for no bound.
        /// </summary>
        public DateTime? Since { get; set; }

        /// <summary>
        /// Gets or sets the UTC date it was queued.
        /// </summary>
        public DateTime EnqueuedOn { get; set; } = DateTime.UtcNow;
    }
}
