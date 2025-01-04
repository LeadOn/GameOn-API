// <copyright file="SubscribeTournamentCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Commands.SubscribeTournament
{
    using GameOn.Application.Common.Players.Queries.GetPlayerByKeycloakId;
    using GameOn.Application.FIFA.Tournaments.Queries.CheckTournamentSubscription;
    using GameOn.Application.FifaTeams.Queries.GetFifaTeamById;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// SubscribeTournamentCommandHandler class.
    /// </summary>
    public class SubscribeTournamentCommandHandler : IRequestHandler<SubscribeTournamentCommand, TournamentPlayer>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeTournamentCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public SubscribeTournamentCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<TournamentPlayer> Handle(SubscribeTournamentCommand request, CancellationToken cancellationToken)
        {
            var isSubscribed = await this.mediator.Send(new CheckTournamentSubscriptionQuery { TournamentId = request.TournamentId, ConnectedPlayer = request.Player });

            if (isSubscribed is not null)
            {
                throw new NotImplementedException();
            }

            var fifaTeamInDb = await this.mediator.Send(new GetFifaTeamByIdQuery { Id = request.FifaTeamId }, cancellationToken);

            if (fifaTeamInDb is null)
            {
                throw new NotImplementedException();
            }

            var playerInDb = await this.mediator.Send(new GetPlayerByKeycloakIdQuery { KeycloakId = request.Player.KeycloakId }, cancellationToken);

            if (playerInDb is null)
            {
                throw new NotImplementedException();
            }

            var tournamentPlayer = new TournamentPlayer
            {
                TournamentId = request.TournamentId,
                PlayerId = playerInDb.Id,
                FifaTeamId = request.FifaTeamId,
            };

            this.context.TournamentPlayers.Add(tournamentPlayer);

            await this.context.SaveChangesAsync(cancellationToken);

            return tournamentPlayer;
        }
    }
}
