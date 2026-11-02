// <copyright file="RefreshValorantPlayerExtrasCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.RefreshValorantPlayerExtras
{
    using MediatR;

    /// <summary>
    /// Refreshes a player's accolades, agent mastery and Premier team (3 low-priority HenrikDev calls).
    /// </summary>
    public class RefreshValorantPlayerExtrasCommand : IRequest<string>
    {
        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
