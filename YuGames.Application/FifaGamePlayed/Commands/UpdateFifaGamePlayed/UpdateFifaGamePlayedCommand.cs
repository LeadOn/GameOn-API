// <copyright file="UpdateFifaGamePlayedCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Commands.UpdateFifaGamePlayed
{
    using MediatR;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

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
