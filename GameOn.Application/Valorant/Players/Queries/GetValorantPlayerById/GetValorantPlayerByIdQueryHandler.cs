// <copyright file="GetValorantPlayerByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerById
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Services;
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetValorantPlayerByIdQueryHandler class.
    /// </summary>
    public class GetValorantPlayerByIdQueryHandler : IRequestHandler<GetValorantPlayerByIdQuery, PlayerDto?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValorantPlayerByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValorantPlayerByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<PlayerDto?> Handle(GetValorantPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players
                .Where(x => x.Id == request.PlayerId
                    && (x.ValorantPuuid != null || x.RiotGamesPUUID != null || (x.RiotGamesNickname != null && x.RiotGamesTagLine != null)))
                .Select(x => new PlayerDto(x))
                .FirstOrDefaultAsync(cancellationToken);

            if (playerInDb is not null)
            {
                var playerHistory = await this.context.ValorantRankHistory
                    .Where(x => x.PlayerId == playerInDb.Id)
                    .OrderByDescending(x => x.CreatedOn)
                    .ThenByDescending(x => x.Id)
                    .ToListAsync(cancellationToken);

                playerInDb.ValorantCurrentRank = playerHistory.FirstOrDefault();
                playerInDb.RrChange7DaysValorant = GetRrChange7Days(playerHistory, DateTime.UtcNow.AddDays(-7));
                playerInDb.RecentFormValorant = await this.context.ValorantGameParticipants
                    .Where(x => x.PlayerId == playerInDb.Id && x.Game.QueueId == "competitive" && !x.Game.IsRemake)
                    .OrderByDescending(x => x.Game.GameStart)
                    .Take(8)
                    .Select(x => x.Win)
                    .ToListAsync(cancellationToken);

                var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
                var mainAgent = await this.context.ValorantGameParticipants
                    .Where(x => x.PlayerId == playerInDb.Id && x.AgentName != null && !x.Game.IsRemake && x.Game.QueueId != "custom" && x.Game.GameStart >= thirtyDaysAgo)
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
                    .FirstOrDefaultAsync(cancellationToken);

                playerInDb.MainAgentName = mainAgent?.AgentName;
                playerInDb.MainAgentIconUrl = string.IsNullOrWhiteSpace(mainAgent?.AgentId)
                    ? null
                    : $"https://media.valorant-api.com/agents/{mainAgent.AgentId}/displayicon.png";

                playerInDb.SmurfAccounts = await this.GetSmurfAccounts(playerInDb.Id, cancellationToken);

                playerInDb.ValorantPerformanceStats = await this.GetPerformanceStats(playerInDb.Id, request, cancellationToken);
            }

            return playerInDb;
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

        private async Task<List<PlayerDto>> GetSmurfAccounts(int playerId, CancellationToken cancellationToken)
        {
            var smurfAccounts = await this.context.Players
                .Where(x => x.PrimaryPlayerId == playerId)
                .OrderBy(x => x.Id)
                .Select(x => new PlayerDto(x))
                .ToListAsync(cancellationToken);

            if (smurfAccounts.Count == 0)
            {
                return smurfAccounts;
            }

            var smurfIds = smurfAccounts.Select(x => x.Id).ToList();
            var ranks = await this.context.ValorantRankHistory
                .Where(x => smurfIds.Contains(x.PlayerId))
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .ToListAsync(cancellationToken);

            foreach (var smurf in smurfAccounts)
            {
                smurf.ValorantCurrentRank = ranks.FirstOrDefault(x => x.PlayerId == smurf.Id);
            }

            return smurfAccounts;
        }

        private async Task<ValorantPlayerPerformanceStatsDto?> GetPerformanceStats(int playerId, GetValorantPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            // GameStart is stored in UTC, so the cutoff uses the same clock.
            DateTime? since = request.Period switch
            {
                LoLStatsPeriod.Week => DateTime.UtcNow.AddDays(-7),
                LoLStatsPeriod.Month => DateTime.UtcNow.AddMonths(-1),
                LoLStatsPeriod.ThreeMonths => DateTime.UtcNow.AddMonths(-3),
                LoLStatsPeriod.SixMonths => DateTime.UtcNow.AddMonths(-6),
                _ => null,
            };

            var query = this.context.ValorantGameParticipants
                .Where(x => x.PlayerId == playerId)
                .CountedGamesOnly();

            if (since is not null)
            {
                query = query.Where(x => x.Game.GameStart >= since);
            }

            // Queue IDs are stored lower case (see UpsertValorantMatchesCommandHandler), so the caller's values
            // are normalized rather than trusted as-is, like on GET val/match.
            var queues = request.Queues?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToLowerInvariant())
                .Distinct()
                .ToList();

            if (queues is { Count: > 0 })
            {
                query = query.Where(x => x.Game.QueueId != null && queues.Contains(x.Game.QueueId));
            }

            if (!string.IsNullOrWhiteSpace(request.AgentName))
            {
                var agentName = request.AgentName.Trim();
                query = query.Where(x => x.AgentName == agentName);
            }

            var lines = await query.Select(ValPerformanceLine.Projection).ToListAsync(cancellationToken);

            if (lines.Count == 0)
            {
                return null;
            }

            var wins = lines.Count(x => x.Win);
            var draws = lines.Count(x => x.IsDraw);
            var totalPlaytimeSeconds = lines.Sum(x => x.GameLengthMs) / 1000.0;
            var linesWithStats = lines.Where(x => x.HasStats).ToList();

            return new ValorantPlayerPerformanceStatsDto
            {
                GamesPlayed = lines.Count,
                Wins = wins,
                Losses = lines.Count - wins - draws,
                Draws = draws,
                WinRatePercent = ValPerformanceMath.WinRate(lines.Count, wins),
                TotalPlaytimeSeconds = (long)totalPlaytimeSeconds,
                AverageGameDurationSeconds = Math.Round(totalPlaytimeSeconds / lines.Count, 1),
                RoundsPlayed = linesWithStats.Sum(x => x.RoundsPlayed),
                AverageAcs = ValPerformanceMath.Acs(lines),
                AverageAdr = ValPerformanceMath.Adr(lines),
                Kda = ValPerformanceMath.Kda(lines),
                KdRatio = ValPerformanceMath.KdRatio(lines),
                HeadshotPercent = ValPerformanceMath.HeadshotPercent(lines),
                KastPercent = ValPerformanceMath.KastPercent(lines),
                AverageRating = linesWithStats.Count > 0 ? Math.Round(linesWithStats.Average(x => x.Rating), 1) : 0,
                FirstKills = lines.Sum(x => x.FirstKills),
                FirstDeaths = lines.Sum(x => x.FirstDeaths),
                AgentStats = ValPerformanceMath.AgentStats<ValAgentStatDto>(lines).Select(x => x.Stat).ToList(),
                MapStats = ValPerformanceMath.MapStats(lines),
                DuoStats = await this.GetDuoStats(playerId, lines, cancellationToken),
            };
        }

        private async Task<List<ValDuoStatDto>> GetDuoStats(int playerId, List<ValPerformanceLine> lines, CancellationToken cancellationToken)
        {
            // This account's team and party in each of its (already filtered) games, to tell teammates from
            // opponents and premades from teammates matched by chance.
            var ownByMatchId = lines
                .GroupBy(x => x.MatchId)
                .ToDictionary(g => g.Key, g => g.First());
            var matchIds = ownByMatchId.Keys.ToList();

            // Every other tracked account in those same matches. Duos are per account, like on LoL: playing with
            // someone's main and with their smurf counts as two partners.
            var teammates = (await this.context.ValorantGameParticipants
                .Where(x => matchIds.Contains(x.MatchId) && x.PlayerId != null && x.PlayerId != playerId)
                .Select(x => new { x.MatchId, x.TeamId, x.PartyId, PlayerId = x.PlayerId!.Value })
                .ToListAsync(cancellationToken))
                .Where(x => x.TeamId == ownByMatchId[x.MatchId].TeamId)
                .GroupBy(x => x.PlayerId)
                .Select(g => new
                {
                    PlayerId = g.Key,
                    GamesPlayed = g.Count(),
                    PremadeGames = g.Count(x => !string.IsNullOrEmpty(x.PartyId) && x.PartyId == ownByMatchId[x.MatchId].PartyId),
                    Wins = g.Count(x => ownByMatchId[x.MatchId].Win),
                })
                .ToList();

            var teammateIds = teammates.Select(x => x.PlayerId).ToList();
            var players = await this.context.Players
                .Where(x => teammateIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            return teammates
                .Where(x => players.ContainsKey(x.PlayerId))
                .Select(x => new ValDuoStatDto
                {
                    Player = new PlayerDto(players[x.PlayerId]),
                    GamesPlayed = x.GamesPlayed,
                    PremadeGames = x.PremadeGames,
                    Wins = x.Wins,
                    WinRate = ValPerformanceMath.WinRate(x.GamesPlayed, x.Wins),
                })
                .OrderByDescending(x => x.GamesPlayed)
                .ThenByDescending(x => x.WinRate)
                .ThenBy(x => x.Player.Id)
                .ToList();
        }
    }
}