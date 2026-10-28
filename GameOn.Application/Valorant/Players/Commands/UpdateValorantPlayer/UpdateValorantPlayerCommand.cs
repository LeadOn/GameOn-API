// <copyright file="UpdateValorantPlayerCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayer
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdateValorantPlayerCommand class.
    /// </summary>
    public class UpdateValorantPlayerCommand : IRequest<PlayerDto>
    {
        /// <summary>
        /// Gets or sets the player to refresh.
        /// </summary>
        public Player Player { get; set; } = new Player();
    }
}