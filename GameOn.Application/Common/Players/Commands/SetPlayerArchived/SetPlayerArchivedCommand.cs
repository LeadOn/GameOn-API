// <copyright file="SetPlayerArchivedCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Commands.SetPlayerArchived
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// SetPlayerArchivedCommand class. Archives or restores a player, touching nothing but
    /// <see cref="Player.Archived"/> -- unlike the full profile update, which also rewrites the Keycloak ID
    /// with whatever the caller sent.
    /// </summary>
    public class SetPlayerArchivedCommand : IRequest<Player?>
    {
        /// <summary>
        /// Gets or sets the ID of the player to archive or restore.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player is archived.
        /// </summary>
        public bool Archived { get; set; }
    }
}
