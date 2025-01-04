// <copyright file="GetTournamentByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Queries.GetTournamentById
{
    using GameOn.Common.DTOs;
    using MediatR;

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
