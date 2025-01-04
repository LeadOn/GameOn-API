// <copyright file="GetTournamentPlayerStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.TournamentPlayers.Queries.GetTournamentPlayerStats
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetTournamentPlayerStatsQuery class.
    /// </summary>
    public class GetTournamentPlayerStatsQuery : IRequest<PlatformStatsDto>
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
