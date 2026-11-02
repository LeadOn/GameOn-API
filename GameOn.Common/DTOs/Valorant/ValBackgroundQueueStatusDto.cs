// <copyright file="ValBackgroundQueueStatusDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// State of the Valorant background queue. Lives in memory: lost on redeploy.
    /// </summary>
    public class ValBackgroundQueueStatusDto
    {
        /// <summary>
        /// Gets or sets the running task, null when idle.
        /// </summary>
        public ValBackgroundTaskDto? InProgress { get; set; }

        /// <summary>
        /// Gets or sets the UTC date the running task started.
        /// </summary>
        public DateTime? InProgressStartedOn { get; set; }

        /// <summary>
        /// Gets or sets the waiting tasks, next first.
        /// </summary>
        public List<ValBackgroundTaskDto> Pending { get; set; } = new List<ValBackgroundTaskDto>();

        /// <summary>
        /// Gets or sets the last finished tasks, newest first.
        /// </summary>
        public List<ValBackgroundTaskResultDto> RecentResults { get; set; } = new List<ValBackgroundTaskResultDto>();
    }
}
