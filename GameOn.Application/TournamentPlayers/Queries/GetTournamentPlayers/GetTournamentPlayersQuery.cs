// <copyright file="GetTournamentPlayersQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.TournamentPlayers.Queries.GetTournamentPlayers
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetTournamentPlayersQuery class.
    /// </summary>
    public class GetTournamentPlayersQuery : IRequest<List<TournamentPlayerDto>>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
