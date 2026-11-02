// <copyright file="RecomputeValGameStatsCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.RecomputeValGameStats
{
    using System;
    using MediatR;

    /// <summary>
    /// Recomputes every derived figure of the stored Valorant matches (sides, zones, kill distances, stats, MVP and
    /// ACE) from the database alone, without any HenrikDev call. Returns the number of matches recomputed.
    /// </summary>
    public class RecomputeValGameStatsCommand : IRequest<int>
    {
        /// <summary>
        /// Gets or sets the oldest match start recomputed, null for every match.
        /// </summary>
        public DateTime? Since { get; set; }
    }
}
