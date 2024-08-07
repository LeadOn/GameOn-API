// <copyright file="GetAllFifaTeamsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaTeams.Queries.GetAllFifaTeams
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllFifaTeamsQuery class.
    /// </summary>
    public class GetAllFifaTeamsQuery : IRequest<IEnumerable<FifaTeam>>
    {
    }
}
