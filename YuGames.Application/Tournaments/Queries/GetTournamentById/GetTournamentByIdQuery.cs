// <copyright file="GetTournamentByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Queries.GetTournamentById
{
    using MediatR;
    using YuGames.Common.DTOs;

    /// <summary>
    /// GetTournamentByIdQuery class.
    /// </summary>
    public class GetTournamentByIdQuery : IRequest<TournamentDto?>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
