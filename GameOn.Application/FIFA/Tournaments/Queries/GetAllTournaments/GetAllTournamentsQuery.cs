// <copyright file="GetAllTournamentsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Queries.GetAllTournaments
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllTournamentsQuery class.
    /// </summary>
    public class GetAllTournamentsQuery : IRequest<IEnumerable<Tournament>>
    {
    }
}
