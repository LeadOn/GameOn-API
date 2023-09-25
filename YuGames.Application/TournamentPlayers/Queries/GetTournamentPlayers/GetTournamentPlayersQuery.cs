// <copyright file="GetTournamentPlayersQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.TournamentPlayers.Queries.GetTournamentPlayers
{
    using MediatR;
    using YuGames.Common.DTOs;

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
