// <copyright file="EnqueueValorantBackgroundTasksCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.EnqueueValorantBackgroundTasks
{
    using System;
    using GameOn.Common.DTOs.Valorant;
    using MediatR;

    /// <summary>
    /// Queues Valorant background tasks. A backfill or an extras refresh without a player is queued once per
    /// Valorant account (archived ones left out); a reimport without a player is a single task covering every match.
    /// </summary>
    public class EnqueueValorantBackgroundTasksCommand : IRequest<ValBackgroundQueueStatusDto>
    {
        /// <summary>
        /// Gets or sets the kind of task.
        /// </summary>
        public ValBackgroundTaskKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the player, null for every Valorant account.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the oldest match start covered (reimport only).
        /// </summary>
        public DateTime? Since { get; set; }
    }
}
