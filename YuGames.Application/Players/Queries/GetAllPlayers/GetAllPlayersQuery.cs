// <copyright file="GetAllPlayersQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Players.Queries.GetAllPlayers
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// GetAllPlayersQuery class.
    /// </summary>
    public class GetAllPlayersQuery : IRequest<IEnumerable<Player>>
    {
    }
}
