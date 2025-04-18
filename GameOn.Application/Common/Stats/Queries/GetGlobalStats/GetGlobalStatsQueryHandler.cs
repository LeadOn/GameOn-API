﻿// <copyright file="GetGlobalStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Stats.Queries.GetGlobalStats
{
    using GameOn.Application.Common.Players.Queries.GetPlayerStats;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetGlobalStatsQueryHandler class.
    /// </summary>
    public class GetGlobalStatsQueryHandler : IRequestHandler<GetGlobalStatsQuery, GlobalStatsDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGlobalStatsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetGlobalStatsQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<GlobalStatsDto> Handle(GetGlobalStatsQuery request, CancellationToken cancellationToken)
        {
            var globalStats = new GlobalStatsDto();

            globalStats.NumberOfGames = await this.context.FifaGamesPlayed.CountAsync(cancellationToken);

            var playersInDb = await this.context.Players.ToListAsync(cancellationToken);

            foreach (var player in playersInDb)
            {
                var stats = await this.mediator.Send(new GetPlayerStatsQuery { PlayerId = player.Id }, cancellationToken);

                if (globalStats.BestPlayer is null || globalStats.BestPlayerStats is null)
                {
                    globalStats.BestPlayer = player;
                    globalStats.BestPlayerStats = stats;
                }

                if (globalStats.WorstPlayer is null || globalStats.WorstPlayerStats is null)
                {
                    globalStats.WorstPlayer = player;
                    globalStats.WorstPlayerStats = stats;
                }

                if (globalStats.BestPlayerStats.StatsPerPlatform![0].WinRate < stats.StatsPerPlatform![0].WinRate)
                {
                    globalStats.BestPlayer = player;
                    globalStats.BestPlayerStats = stats;
                }

                if (globalStats.WorstPlayerStats.StatsPerPlatform![0].LooseRate < stats.StatsPerPlatform[0].LooseRate)
                {
                    globalStats.WorstPlayer = player;
                    globalStats.WorstPlayerStats = stats;
                }
            }

            return globalStats;
        }
    }
}
