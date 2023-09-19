// <copyright file="GetPlayerByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Player.Queries.GetPlayerById
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// GetPlayerByIdQuery class.
    /// </summary>
    public class GetPlayerByIdQuery : IRequest<Player?>
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
