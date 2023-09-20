// <copyright file="GetAllTournamentsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Queries.GetAllTournaments
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// GetAllTournamentsQuery class.
    /// </summary>
    public class GetAllTournamentsQuery : IRequest<IEnumerable<Tournament>>
    {
    }
}
