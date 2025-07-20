// <copyright file="GetTournamentLogoQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Queries.GetTournamentLogo
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetTournamentLogoQuery class.
    /// </summary>
    public class GetTournamentLogoQuery : IRequest<TournamentLogoDto>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
