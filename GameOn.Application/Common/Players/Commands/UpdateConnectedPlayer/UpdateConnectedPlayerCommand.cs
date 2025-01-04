// <copyright file="UpdateConnectedPlayerCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Commands.UpdateConnectedPlayer
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdateConnectedPlayerCommand class.
    /// </summary>
    public class UpdateConnectedPlayerCommand : IRequest<Player>
    {
        /// <summary>
        /// Gets or sets Update player DTO.
        /// </summary>
        public UpdatePlayerDto Player { get; set; } = new UpdatePlayerDto();
    }
}
