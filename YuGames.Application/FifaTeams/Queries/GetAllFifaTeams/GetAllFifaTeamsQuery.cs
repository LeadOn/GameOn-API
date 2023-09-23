// <copyright file="GetAllFifaTeamsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaTeams.Queries.GetAllFifaTeams
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// GetAllFifaTeamsQuery class.
    /// </summary>
    public class GetAllFifaTeamsQuery : IRequest<IEnumerable<FifaTeam>>
    {
    }
}
