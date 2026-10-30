// <copyright file="UpdateValorantPlayerAdminCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayerAdmin
{
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdateValorantPlayerAdminCommand class.
    /// </summary>
    public class UpdateValorantPlayerAdminCommand : IRequest<UpdateValorantPlayerAdminResultDto>
    {
        /// <summary>
        /// Gets or sets the player to update.
        /// </summary>
        public Player Player { get; set; } = new Player();
    }
}