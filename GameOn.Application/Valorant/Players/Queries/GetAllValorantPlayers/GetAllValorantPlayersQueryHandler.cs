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

            var competitiveParticipations = await this.context.ValorantGameParticipants
                .Where(x => x.PlayerId != null && playerIds.Contains(x.PlayerId.Value) && x.Game.QueueId == "competitive" && !x.Game.IsRemake)
                .OrderByDescending(x => x.Game.GameStart)
                .Select(x => new { PlayerId = x.PlayerId!.Value, x.Win })
                .ToListAsync(cancellationToken);

            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var recentAgentParticipations = await this.context.ValorantGameParticipants
                .Where(x => x.PlayerId != null && playerIds.Contains(x.PlayerId.Value) && x.AgentName != null && !x.Game.IsRemake && x.Game.QueueId != "custom" && x.Game.GameStart >= thirtyDaysAgo)
                .Select(x => new { PlayerId = x.PlayerId!.Value, x.AgentName, x.AgentId, x.Win })
                .ToListAsync(cancellationToken);

            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            foreach (var player in playersInDb)
            {
                var history = rankHistory.Where(x => x.PlayerId == player.Id).ToList();
                player.ValorantCurrentRank = history.FirstOrDefault();
                player.RrChange7DaysValorant = GetRrChange7Days(history, sevenDaysAgo);
                player.RecentFormValorant = competitiveParticipations
                    .Where(x => x.PlayerId == player.Id)
                    .Take(5)
                    .Select(x => x.Win)
                    .ToList();

                var mainAgent = recentAgentParticipations
                    .Where(x => x.PlayerId == player.Id)
                    .GroupBy(x => new { x.AgentName, x.AgentId })
                    .Select(g => new
                    {
                        g.Key.AgentName,
                        g.Key.AgentId,
                        Games = g.Count(),
                        Wins = g.Count(x => x.Win),
                    })
                    .OrderByDescending(x => x.Games)
                    .ThenByDescending(x => x.Wins)
                    .ThenBy(x => x.AgentName)
                    .FirstOrDefault();

                player.MainAgentName = mainAgent?.AgentName;
                player.MainAgentIconUrl = string.IsNullOrWhiteSpace(mainAgent?.AgentId)
                    ? null
                    : $"https://media.valorant-api.com/agents/{mainAgent.AgentId}/displayicon.png";
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