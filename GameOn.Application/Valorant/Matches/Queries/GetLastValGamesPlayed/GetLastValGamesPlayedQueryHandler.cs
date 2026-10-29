// <copyright file="GetLastValGamesPlayedQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Queries.GetLastValGamesPlayed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Commands.UpsertValorantMatches;
    using GameOn.Common.DTOs.Common;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="GetLastValGamesPlayedQuery"/> handler.
    /// </summary>
    public class GetLastValGamesPlayedQueryHandler : IRequestHandler<GetLastValGamesPlayedQuery, ListResultDto<ValGame>?>
    {
        private readonly IApplicationDbContext context;
        private readonly IValorantMatchService valorantMatchService;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLastValGamesPlayedQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="valorantMatchService">Valorant Match Service, injected.</param>
        /// <param name="mediator">MediatR sender, injected.</param>
        public GetLastValGamesPlayedQueryHandler(
            IApplicationDbContext context,
            IValorantMatchService valorantMatchService,
            ISender mediator)
        {
            this.context = context;
            this.valorantMatchService = valorantMatchService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<ListResultDto<ValGame>?> Handle(GetLastValGamesPlayedQuery request, CancellationToken cancellationToken)
        {
            var page = Math.Max(1, request.Page);
            var size = Math.Clamp(request.NumberOfResults, 1, 50);

            var query = this.context.ValorantGames
                .Include(x => x.ValorantGameParticipants)
                .ThenInclude(y => y.Stats)
                .Include(x => x.ValorantGameTeams)
                .AsQueryable();

            if (request.PlayerId is null)
            {
                var includeSmurfs = request.IncludeSmurfs;
                var includeOutOfCrew = request.IncludeOutOfCrew;

                query = query.Where(x => x.ValorantGameParticipants.Any(y =>
                    y.PlayerId != null
                    && y.Player != null
                    && (includeOutOfCrew || y.Player.InCrew)
                    && (includeSmurfs || y.Player.PrimaryPlayerId == null)));
            }
            else
            {
                var playerInDb = await this.context.Players
                    .FirstOrDefaultAsync(x => x.Id == request.PlayerId.Value, cancellationToken);

                if (playerInDb is null || string.IsNullOrWhiteSpace(playerInDb.ValorantPuuid))
                {
                    return null;
                }

                var hasStoredMatches = await this.context.ValorantGameParticipants
                    .AnyAsync(x => x.PlayerId == playerInDb.Id, cancellationToken);

                if (request.SyncFromApi || !hasStoredMatches)
                {
                    var region = string.IsNullOrWhiteSpace(playerInDb.ValorantRegion) ? "eu" : playerInDb.ValorantRegion;
                    var recentMatches = await this.valorantMatchService.GetLastMatchesByPuuid(
                        region,
                        playerInDb.ValorantPuuid,
                        size: 10,
                        cancellationToken: cancellationToken);

                    if (recentMatches.Count > 0)
                    {
                        await this.mediator.Send(
                            new UpsertValorantMatchesCommand { Matches = recentMatches },
                            cancellationToken);
                    }
                }

                query = query.Where(x => x.ValorantGameParticipants.Any(y => y.PlayerId == request.PlayerId.Value));

                if (!string.IsNullOrWhiteSpace(request.AgentName))
                {
                    var normalizedAgent = request.AgentName.Trim();
                    query = query.Where(x => x.ValorantGameParticipants.Any(y =>
                        y.PlayerId == request.PlayerId.Value
                        && y.AgentName != null
                        && y.AgentName == normalizedAgent));
                }
            }

            if (request.RankedGamesOnly)
            {
                query = query.Where(x => x.QueueId == "competitive");
            }

            if (request.Queues is { Count: > 0 })
            {
                var normalizedQueues = request.Queues
                    .Where(q => !string.IsNullOrWhiteSpace(q))
                    .Select(q => q.Trim().ToLowerInvariant())
                    .Distinct()
                    .ToList();

                if (normalizedQueues.Count > 0)
                {
                    query = query.Where(x => x.QueueId != null && normalizedQueues.Contains(x.QueueId));
                }
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(x => x.GameStart >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(x => x.GameStart <= request.EndDate.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var results = await query
                .OrderByDescending(x => x.GameStart)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return new ListResultDto<ValGame>
            {
                Page = page,
                Results = results,
                ResultsPerPage = size,
                Total = totalCount,
            };
        }
    }
}
