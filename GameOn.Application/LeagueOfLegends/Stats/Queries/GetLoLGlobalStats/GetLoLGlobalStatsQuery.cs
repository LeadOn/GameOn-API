// <copyright file="GetLoLGlobalStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Stats.Queries.GetLoLGlobalStats
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// GetLoLGlobalStatsQuery class.
    /// </summary>
    public class GetLoLGlobalStatsQuery : IRequest<LoLGlobalStatsDto>
    {
        /// <summary>
        /// Gets or sets a value indicating whether only ranked games (Solo/Duo and Flex) are included.
        /// </summary>
        public bool RankedOnly { get; set; }

        /// <summary>
        /// Gets or sets the queue restriction. Solo or Flex implies ranked games only.
        /// </summary>
        public LoLQueueFilter Queue { get; set; } = LoLQueueFilter.All;

        /// <summary>
        /// Gets or sets the rolling time window restriction.
        /// </summary>
        public LoLStatsPeriod Period { get; set; } = LoLStatsPeriod.AllTime;
    }
}
