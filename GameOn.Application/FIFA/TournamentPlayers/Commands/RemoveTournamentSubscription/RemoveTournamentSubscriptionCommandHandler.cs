// <copyright file="RemoveTournamentSubscriptionCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.TournamentPlayers.Commands.RemoveTournamentSubscription
{
    using GameOn.Application.Players.Queries.GetPlayerById;
    using GameOn.Common.Interfaces;
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
            var playerInDb = await mediator.Send(new GetPlayerByIdQuery { PlayerId = request.PlayerId }, cancellationToken);

            if (playerInDb is null)
            {
                throw new NotImplementedException();
            }

            // First, removing every game related to this user in this tournament
            var userGames = await context.FifaTeamPlayers.Include(x => x.FifaGamePlayed).Where(x => x.PlayerId == request.PlayerId && x.FifaGamePlayed.TournamentId == request.TournamentId).ToListAsync(cancellationToken);

            foreach (var game in userGames)
            {
                context.FifaGamesPlayed.RemoveRange(game.FifaGamePlayed);
            }

            // Now, removing the user from the tournament
            var tournamentPlayer = await context.TournamentPlayers.FirstOrDefaultAsync(x => x.PlayerId == request.PlayerId && x.TournamentId == request.TournamentId, cancellationToken);

            if (tournamentPlayer is null)
            {
                throw new NotImplementedException();
            }

            context.TournamentPlayers.Remove(tournamentPlayer);

            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
