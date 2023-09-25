// <copyright file="CheckTournamentSubscriptionQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Queries.CheckTournamentSubscription
{
    using MediatR;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

    /// <summary>
    /// CheckTournamentSubscriptionQuery class.
    /// </summary>
    public class CheckTournamentSubscriptionQuery : IRequest<TournamentPlayer?>
    {
        /// <summary>
        /// Gets or sets Connected player.
        /// </summary>
        public ConnectedPlayerDto ConnectedPlayer { get; set; } = new ConnectedPlayerDto();

        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
