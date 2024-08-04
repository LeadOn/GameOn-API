// <copyright file="GetAllPlayersQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Queries.GetAllPlayers
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllPlayersQuery class.
    /// </summary>
    public class GetAllPlayersQuery : IRequest<IEnumerable<Player>>
    {
    }
}
