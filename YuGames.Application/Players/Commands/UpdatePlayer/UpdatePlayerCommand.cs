// <copyright file="UpdatePlayerCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Players.Commands.UpdatePlayer
{
    using MediatR;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

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
