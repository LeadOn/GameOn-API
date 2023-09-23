﻿// <copyright file="DeleteFifaGamePlayedCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Commands.DeleteFifaGamePlayed
{
    using MediatR;

    /// <summary>
    /// DeleteFifaGamePlayedCommand class.
    /// </summary>
    public class DeleteFifaGamePlayedCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Game ID.
        /// </summary>
        public int GameId { get; set; }
    }
}
