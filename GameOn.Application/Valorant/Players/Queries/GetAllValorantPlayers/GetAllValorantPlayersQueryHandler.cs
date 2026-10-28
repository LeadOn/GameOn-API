// <copyright file="GetAllValorantPlayersQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetAllValorantPlayers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Common.Players;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllValorantPlayersQueryHandler class.
    /// </summary>
    public class GetAllValorantPlayersQueryHandler : IRequestHandler<GetAllValorantPlayersQuery, IEnumerable<PlayerDto>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllValorantPlayersQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllValorantPlayersQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PlayerDto>> Handle(GetAllValorantPlayersQuery request, CancellationToken cancellationToken)
        {
            var playersQuery = this.context.Players.Include(x => x.TournamentsWon).AsQueryable();

            if (!request.IncludeSmurfs)
            {
                playersQuery = playersQuery.PrimariesOnly();
            }

            if (!request.IncludeOutOfCrew)
            {
                playersQuery = playersQuery.InCrewOnly();
            }

            var playersInDb = await playersQuery
                .Where(x => x.Archived == request.Archived
                    && (x.ValorantPuuid != null || (x.RiotGamesNickname != null && x.RiotGamesTagLine != null)))
                .Select(x => new PlayerDto(x))
                .ToListAsync(cancellationToken);

            if (playersInDb.Count == 0)
            {
                return playersInDb;
            }

            var playerIds = playersInDb.Select(x => x.Id).ToList();
            var rankHistory = await this.context.ValorantRankHistory
                .Where(x => playerIds.Contains(x.PlayerId))
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .ToListAsync(cancellationToken);

            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            foreach (var player in playersInDb)
            {
                var history = rankHistory.Where(x => x.PlayerId == player.Id).ToList();
                player.ValorantCurrentRank = history.FirstOrDefault();
                player.RrChange7DaysValorant = GetRrChange7Days(history, sevenDaysAgo);
            }

            return playersInDb;
        }

        private static int? GetRrChange7Days(List<ValorantRankHistory> historyDescending, DateTime sevenDaysAgo)
        {
            var current = historyDescending.FirstOrDefault(x => x.TierId >= 3 && x.Elo > 0);
            var baseline = historyDescending.FirstOrDefault(x => x.CreatedOn <= sevenDaysAgo && x.TierId >= 3 && x.Elo > 0);

            if (current is null || baseline is null)
            {
                return null;
            }

            return current.Elo - baseline.Elo;
        }
    }
}