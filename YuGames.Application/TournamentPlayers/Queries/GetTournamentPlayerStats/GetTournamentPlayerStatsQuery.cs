// <copyright file="GetTournamentPlayerStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.TournamentPlayers.Queries.GetTournamentPlayerStats
{
    using MediatR;
    using YuGames.Common.DTOs;

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
