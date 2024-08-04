// <copyright file="UpdateFifaGamePlayedCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Commands.UpdateFifaGamePlayed
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdateFifaGamePlayedCommand class.
    /// </summary>
    public class UpdateFifaGamePlayedCommand : IRequest<FifaGamePlayed?>
    {
        /// <summary>
        /// Gets or sets UpdateGameDto.
        /// </summary>
        public UpdateGameDto Game { get; set; } = new UpdateGameDto();
    }
}
