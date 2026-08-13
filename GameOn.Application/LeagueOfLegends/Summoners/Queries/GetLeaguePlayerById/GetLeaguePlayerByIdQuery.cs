// <copyright file="GetLeaguePlayerByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetLeaguePlayerById
{
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetLeaguePlayerByIdQuery class.
    /// </summary>
    public class GetLeaguePlayerByIdQuery : IRequest<PlayerDto?>
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the rolling time window for <see cref="PlayerDto.PerformanceStats"/>.
        /// </summary>
        public LoLStatsPeriod Period { get; set; } = LoLStatsPeriod.AllTime;
    }
}
