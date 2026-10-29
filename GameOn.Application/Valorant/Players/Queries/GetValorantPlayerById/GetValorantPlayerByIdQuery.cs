// <copyright file="GetValorantPlayerByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerById
{
    using System.Collections.Generic;
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// GetValorantPlayerByIdQuery class.
    /// </summary>
    public class GetValorantPlayerByIdQuery : IRequest<PlayerDto?>
    {
        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the rolling time window for <see cref="PlayerDto.ValorantPerformanceStats"/>.
        /// </summary>
        public LoLStatsPeriod Period { get; set; } = LoLStatsPeriod.AllTime;

        /// <summary>
        /// Gets or sets the HenrikDev queue IDs (e.g. "competitive", "unrated") to restrict
        /// <see cref="PlayerDto.ValorantPerformanceStats"/> to. Null or empty means every counted queue.
        /// </summary>
        public List<string>? Queues { get; set; }

        /// <summary>
        /// Gets or sets the agent display name (e.g. "Jett") to restrict
        /// <see cref="PlayerDto.ValorantPerformanceStats"/> to. Null or empty means every agent.
        /// </summary>
        public string? AgentName { get; set; }
    }
}
