// <copyright file="GetFeaturedTournamentsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Queries.GetFeaturedTournaments
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetFeaturedTournamentsQuery class.
    /// </summary>
    public class GetFeaturedTournamentsQuery : IRequest<IEnumerable<TournamentDto>>
    {
    }
}
