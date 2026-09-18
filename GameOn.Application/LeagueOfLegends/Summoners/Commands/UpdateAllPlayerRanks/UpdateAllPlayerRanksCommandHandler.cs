// <copyright file="UpdateAllPlayerRanksCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdateAllPlayerRanks
{
    using GameOn.Application.Common.Players;
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummoner;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateAllPlayerRanksCommandHandler class.
    /// </summary>
    public class UpdateAllPlayerRanksCommandHandler : IRequestHandler<UpdateAllPlayerRanksCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAllPlayerRanksCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        public UpdateAllPlayerRanksCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task Handle(UpdateAllPlayerRanksCommand request, CancellationToken cancellationToken)
        {
            // Crew accounts only: this is the automatic refresh, and an account outside the crew is one
            // we deliberately stopped tracking. It stays refreshable on demand through
            // PATCH lol/Summoner/{id}, which goes straight to UpdatePlayerSummonerCommand.
            var playersToRefresh = await this.context.Players
                .InCrewOnly()
                .Where(x => x.RiotGamesPUUID != null && x.RiotGamesPUUID != string.Empty)
                .ToListAsync(cancellationToken);

            foreach (var playerInDb in playersToRefresh)
            {
                await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = playerInDb }, cancellationToken);
            }
        }
    }
}
