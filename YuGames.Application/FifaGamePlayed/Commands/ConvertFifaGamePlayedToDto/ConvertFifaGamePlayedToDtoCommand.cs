// <copyright file="ConvertFifaGamePlayedToDtoCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Commands.ConvertFifaGamePlayedToDto
{
    using MediatR;
    using YuGames.Common.DTOs;

    /// <summary>
    /// ConvertFifaGamePlayedToDto class.
    /// </summary>
    public class ConvertFifaGamePlayedToDtoCommand : IRequest<FifaGamePlayedDto>
    {
        /// <summary>
        /// Gets or sets FifaGamePlayed.
        /// </summary>
        public Domain.FifaGamePlayed FifaGamePlayed { get; set; } = new Domain.FifaGamePlayed();
    }
}
