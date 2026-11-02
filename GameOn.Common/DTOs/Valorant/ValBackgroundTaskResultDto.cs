// <copyright file="ValBackgroundTaskResultDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System;

    /// <summary>
    /// Outcome of a finished Valorant background task.
    /// </summary>
    public class ValBackgroundTaskResultDto
    {
        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        public ValBackgroundTaskDto Task { get; set; } = new ValBackgroundTaskDto();

        /// <summary>
        /// Gets or sets the UTC date it started.
        /// </summary>
        public DateTime StartedOn { get; set; }

        /// <summary>
        /// Gets or sets the UTC date it finished.
        /// </summary>
        public DateTime FinishedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it stopped on an error.
        /// </summary>
        public bool Failed { get; set; }

        /// <summary>
        /// Gets or sets what it did, or the error that stopped it.
        /// </summary>
        public string Summary { get; set; } = string.Empty;
    }
}
