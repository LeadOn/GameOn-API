// <copyright file="UpdateConnectedPlayerCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Commands.UpdateConnectedPlayer
{
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.Common;
    using MediatR;

    /// <summary>
    /// UpdateConnectedPlayerCommand class.
    /// </summary>
    public class UpdateConnectedPlayerCommand : IRequest<UpdateConnectedPlayerResultDto>
    {
        /// <summary>
        /// Gets or sets Update player DTO.
        /// </summary>
        public UpdatePlayerDto Player { get; set; } = new UpdatePlayerDto();
    }
}
