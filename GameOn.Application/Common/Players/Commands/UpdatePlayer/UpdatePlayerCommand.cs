// <copyright file="UpdatePlayerCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Commands.UpdatePlayer
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdatePlayerCommand class.
    /// </summary>
    public class UpdatePlayerCommand : IRequest<Player>
    {
        /// <summary>
        /// Gets or sets Update player DTO.
        /// </summary>
        public UpdatePlayerDto Player { get; set; } = new UpdatePlayerDto();
    }
}
