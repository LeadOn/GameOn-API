// <copyright file="UpdateAllValorantPlayerRanksCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Commands.UpdateAllValorantPlayerRanks
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Common.Players;
    using GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayer;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateAllValorantPlayerRanksCommandHandler class.
    /// </summary>
    public class UpdateAllValorantPlayerRanksCommandHandler : IRequestHandler<UpdateAllValorantPlayerRanksCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAllValorantPlayerRanksCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        public UpdateAllValorantPlayerRanksCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task Handle(UpdateAllValorantPlayerRanksCommand request, CancellationToken cancellationToken)
        {
            var playersToRefresh = await this.context.Players
                .InCrewOnly()
                .Where(x => !x.Archived
                    && (x.ValorantPuuid != null || (x.RiotGamesNickname != null && x.RiotGamesTagLine != null)))
                .ToListAsync(cancellationToken);

            foreach (var playerInDb in playersToRefresh)
            {
                await this.mediator.Send(new UpdateValorantPlayerCommand { Player = playerInDb }, cancellationToken);

                // Paced so a full crew refresh stays well within HenrikDev's per-minute rate limit.
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
        }
    }
}