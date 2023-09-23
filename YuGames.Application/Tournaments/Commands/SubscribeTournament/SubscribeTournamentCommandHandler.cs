// <copyright file="SubscribeTournamentCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Commands.SubscribeTournament
{
    using MediatR;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Application.FifaTeams.Queries.GetFifaTeamById;
    using YuGames.Application.Players.Queries.GetPlayerById;
    using YuGames.Application.Players.Queries.GetPlayerByKeycloakId;
    using YuGames.Application.Tournaments.Queries.CheckTournamentSubscription;
    using YuGames.Domain;

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

            if (isSubscribed == true)
            {
                throw new NotImplementedException();
            }

            var fifaTeamInDb = await this.mediator.Send(new GetFifaTeamByIdQuery { Id = request.FifaTeamId });

            if (fifaTeamInDb is null)
            {
                throw new NotImplementedException();
            }

            var playerInDb = await this.mediator.Send(new GetPlayerByKeycloakIdQuery { KeycloakId = request.Player.KeycloakId });

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
