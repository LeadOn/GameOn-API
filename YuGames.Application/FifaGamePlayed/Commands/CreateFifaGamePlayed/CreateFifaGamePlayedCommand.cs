// <copyright file="CreateFifaGamePlayedCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Commands.CreateFifaGamePlayed
{
    using MediatR;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

    /// <summary>
    /// CreateFifaGamePlayedCommand class.
    /// </summary>
    public class CreateFifaGamePlayedCommand : IRequest<FifaGamePlayed?>
    {
        /// <summary>
        /// Gets or sets CreateFifaGameDto.
        /// </summary>
        public CreateFifaGameDto NewGame { get; set; } = new CreateFifaGameDto();
    }
}
