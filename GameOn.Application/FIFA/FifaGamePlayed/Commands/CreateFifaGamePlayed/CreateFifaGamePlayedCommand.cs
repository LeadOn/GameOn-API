// <copyright file="CreateFifaGamePlayedCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaGamePlayed.Commands.CreateFifaGamePlayed
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

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
