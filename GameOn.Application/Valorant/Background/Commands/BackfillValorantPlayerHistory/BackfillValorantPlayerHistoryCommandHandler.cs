// <copyright file="BackfillValorantPlayerHistoryCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.BackfillValorantPlayerHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Commands.UpsertValorantMatches;
    using GameOn.Application.Valorant.Players.Services;
    using GameOn.Common.Interfaces;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// <see cref="BackfillValorantPlayerHistoryCommand"/> handler.
    /// </summary>
    /// <remarks>
    /// HenrikDev's stored history is an accumulated subset of Riot's (a match is there only if somebody ever
    /// asked for it), so the oldest matches may be missing: nothing can go further back. Each match is imported
    /// in its own DI scope, hence its own DbContext: a match tracks a few thousand rows (positions, damages),
    /// and one context across a hundred matches would slow every save down to a crawl.
    /// </remarks>
    public class BackfillValorantPlayerHistoryCommandHandler : IRequestHandler<BackfillValorantPlayerHistoryCommand, string>
    {
        private const int MmrPageSize = 100;

        private const int MaxMmrPages = 50;

        private readonly IApplicationDbContext context;
        private readonly IValorantStoredDataService storedDataService;
        private readonly IValorantMatchService matchService;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<BackfillValorantPlayerHistoryCommandHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackfillValorantPlayerHistoryCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="storedDataService">HenrikDev stored data, injected.</param>
        /// <param name="matchService">HenrikDev matches, injected.</param>
        /// <param name="scopeFactory">Scope factory, injected.</param>
        /// <param name="logger">Logger, injected.</param>
        public BackfillValorantPlayerHistoryCommandHandler(
            IApplicationDbContext context,
            IValorantStoredDataService storedDataService,
            IValorantMatchService matchService,
            IServiceScopeFactory scopeFactory,
            ILogger<BackfillValorantPlayerHistoryCommandHandler> logger)
        {
            this.context = context;
            this.storedDataService = storedDataService;
            this.matchService = matchService;
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<string> Handle(BackfillValorantPlayerHistoryCommand request, CancellationToken cancellationToken)
        {
            var player = await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);

            if (string.IsNullOrWhiteSpace(player?.ValorantPuuid))
            {
                return $"Player {request.PlayerId} has no Valorant account.";
            }

            var region = string.IsNullOrWhiteSpace(player.ValorantRegion) ? "eu" : player.ValorantRegion;

            // 1. MMR history, before the matches: the import links each participation to its RR change.
            var entries = new List<ValorantMmrHistoryEntryDto>();

            for (var page = 1; page <= MaxMmrPages; page++)
            {
                var response = await this.storedDataService.GetStoredMmrHistory(region, player.ValorantPuuid, MmrPageSize, page, cancellationToken);

                if (response is null || response.Data.Count == 0)
                {
                    break;
                }

                entries.AddRange(response.Data);

                if ((response.Results?.After ?? 0) == 0)
                {
                    break;
                }
            }

            var rankEntriesBefore = await this.context.ValorantRankHistory.CountAsync(x => x.PlayerId == player.Id && x.MatchId != null, cancellationToken);
            await ValorantRankHistorySync.Upsert(this.context, player.Id, entries, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);
            var rankEntriesAfter = await this.context.ValorantRankHistory.CountAsync(x => x.PlayerId == player.Id && x.MatchId != null, cancellationToken);

            // 2. Every stored match missing from the database, newest first.
            var stored = await this.storedDataService.GetStoredMatches(region, player.ValorantPuuid, null, null, cancellationToken);
            var storedIds = (stored?.Data ?? new List<ValorantStoredMatchDto>())
                .Select(x => x.Meta?.Id)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var known = await this.context.ValorantGames
                .Where(x => storedIds.Contains(x.MatchId))
                .Select(x => x.MatchId)
                .ToListAsync(cancellationToken);

            var knownSet = new HashSet<string>(known, StringComparer.OrdinalIgnoreCase);
            var missing = storedIds.Where(x => !knownSet.Contains(x)).ToList();
            int imported = 0, notFound = 0, failed = 0;

            foreach (var matchId in missing)
            {
                try
                {
                    var match = await this.matchService.GetMatchById(region, matchId, cancellationToken, lowPriority: true);

                    if (match is null)
                    {
                        notFound++;
                        continue;
                    }

                    using var scope = this.scopeFactory.CreateScope();
                    var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                    await sender.Send(new UpsertValorantMatchesCommand { Matches = new List<ValorantMatchDto> { match } }, cancellationToken);
                    imported++;
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    failed++;
                    this.logger.LogWarning(ex, "Valorant backfill: match {MatchId} of player {PlayerId} could not be imported.", matchId, player.Id);
                }
            }

            return $"Player {player.Id}: {rankEntriesAfter - rankEntriesBefore} rank entries added ({entries.Count} stored), "
                + $"{storedIds.Count} stored matches, {known.Count} already known, {imported} imported, {notFound} not found, {failed} failed.";
        }
    }
}
