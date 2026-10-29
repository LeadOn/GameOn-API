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
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// UpdateAllValorantPlayerRanksCommandHandler class.
    /// </summary>
    public class UpdateAllValorantPlayerRanksCommandHandler : IRequestHandler<UpdateAllValorantPlayerRanksCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;
        private readonly ILogger<UpdateAllValorantPlayerRanksCommandHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAllValorantPlayerRanksCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        /// <param name="logger">Logger, injected.</param>
        public UpdateAllValorantPlayerRanksCommandHandler(IApplicationDbContext context, ISender mediator, ILogger<UpdateAllValorantPlayerRanksCommandHandler> logger)
        {
            this.context = context;
            this.mediator = mediator;
            this.logger = logger;
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
                // One account failing (HenrikDev down, an unknown Riot ID answered with a 5xx...) must not
                // cost every account after it its refresh: this is a batch, not a single request, so the
                // failure is logged and the round moves on. The HenrikDev quota itself is handled upstream,
                // by the rate limit handler, which holds requests back instead of letting them fail.
                try
                {
                    await this.mediator.Send(new UpdateValorantPlayerCommand { Player = playerInDb }, cancellationToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    this.logger.LogError(ex, "Valorant refresh failed for player {PlayerId}, moving on to the next one.", playerInDb.Id);
                }

                // Spreads a full crew refresh over time rather than draining the shared quota in one burst,
                // which would leave a player's manual refresh waiting for the window to reset.
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
        }
    }
}