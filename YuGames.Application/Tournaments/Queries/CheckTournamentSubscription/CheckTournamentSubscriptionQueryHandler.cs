// <copyright file="CheckTournamentSubscriptionQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Queries.CheckTournamentSubscription
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;

    /// <summary>
    /// CheckTournamentSubscriptionQueryHandler class.
    /// </summary>
    public class CheckTournamentSubscriptionQueryHandler : IRequestHandler<CheckTournamentSubscriptionQuery, bool>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckTournamentSubscriptionQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public CheckTournamentSubscriptionQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(CheckTournamentSubscriptionQuery request, CancellationToken cancellationToken)
        {
            // Getting player
            var playerInDb =
                await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == request.ConnectedPlayer.KeycloakId, cancellationToken);

            if (playerInDb is null)
            {
                return false;
            }

            var playerSubscription = await this.context.TournamentPlayers.FirstOrDefaultAsync(x => x.TournamentId == request.TournamentId && x.PlayerId == playerInDb.Id, cancellationToken);

            if (playerSubscription is not null)
            {
                return true;
            }

            return false;
        }
    }
}
