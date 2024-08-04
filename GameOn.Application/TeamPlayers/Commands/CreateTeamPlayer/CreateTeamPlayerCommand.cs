// <copyright file="CreateTeamPlayerCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.TeamPlayers.Commands.CreateTeamPlayer
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// CreateTeamPlayerCommand class.
    /// </summary>
    public class CreateTeamPlayerCommand : IRequest<FifaTeamPlayer>
    {
        /// <summary>
        /// Gets or sets Team Player.
        /// </summary>
        public FifaTeamPlayer TeamPlayer { get; set; } = new FifaTeamPlayer();
    }
}
