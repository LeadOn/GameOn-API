﻿// <copyright file="UpdateTournamentSubscriptionCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.TournamentPlayers.Commands.UpdateTournamentSubscription
{
    using GameOn.Application.Common.Players.Queries.GetPlayerByKeycloakId;
    using GameOn.Application.FIFA.FifaTeams.Queries.GetFifaTeamById;
    using GameOn.Application.FIFA.Tournaments.Queries.CheckTournamentSubscription;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateTournamentSubscriptionCommandHandler class.
    /// </summary>
    public class UpdateTournamentSubscriptionCommandHandler : IRequestHandler<UpdateTournamentSubscriptionCommand, TournamentPlayer?>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTournamentSubscriptionCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public UpdateTournamentSubscriptionCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<TournamentPlayer?> Handle(UpdateTournamentSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByKeycloakIdQuery { KeycloakId = request.Player.KeycloakId }, cancellationToken);

            if (playerInDb is null)
            {
                throw new NotImplementedException();
            }

            var isSubscribed = await this.mediator.Send(new CheckTournamentSubscriptionQuery { TournamentId = request.TournamentId, ConnectedPlayer = request.Player }, cancellationToken);

            if (isSubscribed is null)
            {
                throw new NotImplementedException();
            }

            var fifaTeamInDb = await this.mediator.Send(new GetFifaTeamByIdQuery { Id = request.FifaTeamId }, cancellationToken);

            if (fifaTeamInDb is null)
            {
                throw new NotImplementedException();
            }

            var tournamentInDb = await this.context.Tournaments.FirstOrDefaultAsync(x => x.Id == request.TournamentId, cancellationToken);

            if (tournamentInDb is null)
            {
                throw new NotImplementedException();
            }

            if (tournamentInDb.State >= TournamentStates.Phase1)
            {
                throw new NotImplementedException();
            }

            isSubscribed.FifaTeamId = request.FifaTeamId;

            this.context.TournamentPlayers.Update(isSubscribed);

            await this.context.SaveChangesAsync(cancellationToken);

            return isSubscribed;
        }
    }
}
