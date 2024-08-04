// <copyright file="GetPlayerByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Queries.GetPlayerById
{
    using GameOn.Domain;
    using MediatR;

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
