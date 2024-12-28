// <copyright file="RemoveTournamentSubscriptionCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.TournamentPlayers.Commands.RemoveTournamentSubscription
{
    using GameOn.Common.Interfaces;
    using GameOn.Application.Players.Queries.GetPlayerById;
    using GameOn.Application.Players.Queries.GetPlayerByKeycloakId;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// RemoveTournamentSubscriptionCommandHandler class.
    /// </summary>
    public class RemoveTournamentSubscriptionCommandHandler : IRequestHandler<RemoveTournamentSubscriptionCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveTournamentSubscriptionCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public RemoveTournamentSubscriptionCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(RemoveTournamentSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = request.PlayerId }, cancellationToken);

            if (playerInDb is null)
            {
                throw new NotImplementedException();
            }

            // First, removing every game related to this user in this tournament
            var userGames = await this.context.FifaTeamPlayers.Include(x => x.FifaGamePlayed).Where(x => x.PlayerId == request.PlayerId && x.FifaGamePlayed.TournamentId == request.TournamentId).ToListAsync(cancellationToken);

            foreach (var game in userGames)
            {
                this.context.FifaGamesPlayed.RemoveRange(game.FifaGamePlayed);
            }

            // Now, removing the user from the tournament
            var tournamentPlayer = await this.context.TournamentPlayers.FirstOrDefaultAsync(x => x.PlayerId == request.PlayerId && x.TournamentId == request.TournamentId, cancellationToken);

            if (tournamentPlayer is null)
            {
                throw new NotImplementedException();
            }

            this.context.TournamentPlayers.Remove(tournamentPlayer);

            await this.context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
