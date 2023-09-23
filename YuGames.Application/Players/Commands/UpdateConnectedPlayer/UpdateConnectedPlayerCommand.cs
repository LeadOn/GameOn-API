// <copyright file="UpdateConnectedPlayerCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Players.Commands.UpdateConnectedPlayer
{
    using MediatR;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

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
