// <copyright file="GetLeaguePlayerByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetLeaguePlayerById
{
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLeaguePlayerByIdQueryHandler class.
    /// </summary>
    public class GetLeaguePlayerByIdQueryHandler : IRequestHandler<GetLeaguePlayerByIdQuery, PlayerDto?>
    {
        // Matched against LoLQueue.Map + Description (synced from Riot) to keep only games against real
        // opponents. Duplicated from GetLoLGlobalStatsQueryHandler: worth factoring out into a shared
        // helper if a third caller shows up.
        private static readonly string[] ExcludedQueueTypeKeywords = { "Co-op", "Bot", "Tutorial", "Custom" };

        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLeaguePlayerByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetLeaguePlayerByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<PlayerDto?> Handle(GetLeaguePlayerByIdQuery request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players.Where(x => x.Id == request.PlayerId && x.RiotGamesPUUID != null).Select(x => new PlayerDto(x)).FirstOrDefaultAsync(cancellationToken);

            if (playerInDb != null)
            {
                var soloRank = await this.context.LeagueOfLegendsRankHistory.OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync(x => x.PlayerId == playerInDb.Id && x.QueueType == "RANKED_SOLO_5x5", cancellationToken);

                playerInDb.LeagueOfLegendsSoloRank = soloRank;

                var flexRank = await this.context.LeagueOfLegendsRankHistory.OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync(x => x.PlayerId == playerInDb.Id && x.QueueType == "RANKED_FLEX_SR", cancellationToken);

                playerInDb.LeagueOfLegendsFlexRank = flexRank;

                playerInDb.PerformanceStats = await this.GetPerformanceStats(playerInDb.Id, request.Period, cancellationToken);
            }

            return playerInDb;
        }

        private async Task<LoLSummonerPerformanceStatsDto?> GetPerformanceStats(int playerId, LoLStatsPeriod period, CancellationToken cancellationToken)
        {
            // GameStart is stored in UTC (see GetLoLGlobalStatsQueryHandler), so the cutoff uses the same clock.
            DateTime? since = period switch
            {
                LoLStatsPeriod.Week => DateTime.UtcNow.AddDays(-7),
                LoLStatsPeriod.Month => DateTime.UtcNow.AddMonths(-1),
                LoLStatsPeriod.ThreeMonths => DateTime.UtcNow.AddMonths(-3),
                LoLStatsPeriod.SixMonths => DateTime.UtcNow.AddMonths(-6),
                _ => null,
            };

            // Games whose queue could not be resolved are dropped: without a LoLQueue row there is no way
            // to tell a real game from a bot or custom one (see GetLoLGlobalStatsQueryHandler).
            var query = this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.PlayerId == playerId && x.ChampionName != string.Empty && x.Game.Queue != null && !x.Game.IsRemake);

            if (since is not null)
            {
                query = query.Where(x => x.Game.GameStart >= since);
            }

            var games = (await query
                .Select(x => new
                {
                    x.Win,
                    x.Game.GameStart,
                    x.Game.GameEnd,
                    QueueMap = x.Game.Queue!.Map,
                    QueueDescription = x.Game.Queue!.Description,
                    Kda = x.Stats != null ? x.Stats.Kda : (double?)null,
                    CsPerMinute = x.Stats != null ? x.Stats.CsPerMinute : (double?)null,
                    DamagePerMinute = x.Stats != null ? x.Stats.DamagePerMinute : (double?)null,
                    x.VisionScore,
                })
                .ToListAsync(cancellationToken))
                .Where(x => !ExcludedQueueTypeKeywords.Any(keyword =>
                    ((x.QueueMap ?? string.Empty) + " " + (x.QueueDescription ?? string.Empty))
                        .Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (games.Count == 0)
            {
                return null;
            }

            var totalPlaytimeSeconds = games.Sum(x => (x.GameEnd - x.GameStart).TotalSeconds);
            var wins = games.Count(x => x.Win);

            return new LoLSummonerPerformanceStatsDto
            {
                GamesPlayed = games.Count,
                Wins = wins,
                Losses = games.Count - wins,
                WinRatePercent = Math.Round(100.0 * wins / games.Count, 1),
                TotalPlaytimeSeconds = (long)totalPlaytimeSeconds,
                AverageGameDurationSeconds = Math.Round(totalPlaytimeSeconds / games.Count, 1),
                AverageKda = Math.Round(games.Select(x => x.Kda).Where(x => x is not null).Select(x => x!.Value).DefaultIfEmpty(0).Average(), 2),
                AverageCsPerMinute = Math.Round(games.Select(x => x.CsPerMinute).Where(x => x is not null).Select(x => x!.Value).DefaultIfEmpty(0).Average(), 1),
                AverageDamagePerMinute = Math.Round(games.Select(x => x.DamagePerMinute).Where(x => x is not null).Select(x => x!.Value).DefaultIfEmpty(0).Average(), 1),
                AverageVisionScore = Math.Round(games.Average(x => x.VisionScore), 1),
            };
        }
    }
}
