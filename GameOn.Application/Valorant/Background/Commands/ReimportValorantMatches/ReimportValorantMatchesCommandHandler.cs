// <copyright file="ReimportValorantMatchesCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.ReimportValorantMatches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Commands.UpsertValorantMatches;
    using GameOn.Common.Interfaces;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// <see cref="ReimportValorantMatchesCommand"/> handler. Each match is re-imported in its own DI scope, for
    /// the same reason as the backfill: one DbContext across many matches would track far too many rows.
    /// </summary>
    public class ReimportValorantMatchesCommandHandler : IRequestHandler<ReimportValorantMatchesCommand, string>
    {
        private readonly IApplicationDbContext context;
        private readonly IValorantMatchService matchService;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<ReimportValorantMatchesCommandHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReimportValorantMatchesCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="matchService">HenrikDev matches, injected.</param>
        /// <param name="scopeFactory">Scope factory, injected.</param>
        /// <param name="logger">Logger, injected.</param>
        public ReimportValorantMatchesCommandHandler(
            IApplicationDbContext context,
            IValorantMatchService matchService,
            IServiceScopeFactory scopeFactory,
            ILogger<ReimportValorantMatchesCommandHandler> logger)
        {
            this.context = context;
            this.matchService = matchService;
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<string> Handle(ReimportValorantMatchesCommand request, CancellationToken cancellationToken)
        {
            var query = this.context.ValorantGames.AsQueryable();

            if (request.PlayerId is { } playerId)
            {
                query = query.Where(x => x.ValorantGameParticipants.Any(p => p.PlayerId == playerId));
            }

            if (request.Since is { } since)
            {
                query = query.Where(x => x.GameStart >= since);
            }

            var games = await query
                .OrderByDescending(x => x.GameStart)
                .Select(x => new { x.MatchId, x.Region })
                .ToListAsync(cancellationToken);

            int reimported = 0, notFound = 0, failed = 0;

            foreach (var game in games)
            {
                try
                {
                    var match = await this.matchService.GetMatchById(game.Region ?? "eu", game.MatchId, cancellationToken, lowPriority: true);

                    if (match is null)
                    {
                        notFound++;
                        continue;
                    }

                    using var scope = this.scopeFactory.CreateScope();
                    var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                    await sender.Send(
                        new UpsertValorantMatchesCommand { Matches = new List<ValorantMatchDto> { match }, OverwriteExisting = true },
                        cancellationToken);
                    reimported++;
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    failed++;
                    this.logger.LogWarning(ex, "Valorant reimport: match {MatchId} could not be re-imported.", game.MatchId);
                }
            }

            return $"{games.Count} matches selected, {reimported} re-imported, {notFound} not found, {failed} failed.";
        }
    }
}
