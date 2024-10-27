// <copyright file="RemoveTournamentSubscriptionCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.TournamentPlayers.Commands.RemoveTournamentSubscription
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// RemoveTournamentSubscriptionCommand class.
    /// </summary>
    public class RemoveTournamentSubscriptionCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets ConnectedPlayer.
        /// </summary>
        public ConnectedPlayerDto Player { get; set; } = new ConnectedPlayerDto();

        /// <summary>
        /// Gets or sets PlayerId.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
