// <copyright file="RegisterPlayerCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Commands.RegisterPlayer
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// RegisterPlayerCommand class.
    /// </summary>
    public class RegisterPlayerCommand : IRequest<Player>
    {
        /// <summary>
        /// Gets or sets Player.
        /// </summary>
        public Player Player { get; set; } = new Player();
    }
}
