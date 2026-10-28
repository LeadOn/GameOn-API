// <copyright file="GetValorantPlayerByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerById
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetValorantPlayerByIdQuery class.
    /// </summary>
    public class GetValorantPlayerByIdQuery : IRequest<PlayerDto?>
    {
        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}