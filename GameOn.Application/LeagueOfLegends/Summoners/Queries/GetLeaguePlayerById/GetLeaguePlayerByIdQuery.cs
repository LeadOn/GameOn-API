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

        /// <summary>
        /// Gets or sets the Riot queue IDs (see <see cref="Domain.LoLQueue"/>) to restrict
        /// <see cref="PlayerDto.PerformanceStats"/> to. Null or empty means every queue.
        /// </summary>
        public List<int>? QueueIds { get; set; }

        /// <summary>
        /// Gets or sets the Riot team position (TOP, JUNGLE, MIDDLE, BOTTOM, UTILITY) to restrict
        /// <see cref="PlayerDto.PerformanceStats"/> to. Null or empty means every role.
        /// </summary>
        public string? TeamPosition { get; set; }
    }
}
