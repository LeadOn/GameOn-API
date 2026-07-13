// <copyright file="UpdateAllPlayerRanksCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdateAllPlayerRanks
{
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
            foreach (var playerInDb in await this.context.Players.Where(x => x.RiotGamesPUUID != null && x.RiotGamesPUUID != string.Empty).ToListAsync(cancellationToken))
            {
                await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = playerInDb }, cancellationToken);
            }
        }
    }
}
