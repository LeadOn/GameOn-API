// <copyright file="GetAllPlayersQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Queries.GetAllPlayers
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllPlayersQuery class.
    /// </summary>
    public class GetAllPlayersQuery : IRequest<IEnumerable<Player>>
    {
        /// <summary>
        /// Gets or sets a value indicating whether a player is archived or not.
        /// </summary>
        public bool Archived { get; set; } = false;
    }
}
