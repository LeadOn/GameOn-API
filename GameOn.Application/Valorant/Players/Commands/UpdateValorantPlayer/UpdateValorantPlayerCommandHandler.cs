// <copyright file="UpdateValorantPlayerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Commands.UpsertValorantMatches;
    using GameOn.Application.Valorant.Players.Services;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// UpdateValorantPlayerCommandHandler class.
    /// </summary>
    public class UpdateValorantPlayerCommandHandler : IRequestHandler<UpdateValorantPlayerCommand, PlayerDto>
    {
        /// <summary>
        /// Matches per HenrikDev page (v4 caps the page size at 10).
        /// </summary>
        private const int MatchPageSize = 10;

        /// <summary>
        /// Pages read at most by one refresh when every match of a page is unknown.
        /// </summary>
        private const int MaxGapPages = 3;

        private readonly IApplicationDbContext context;
        private readonly IValorantAccountService valorantAccountService;
        private readonly IValorantMmrService valorantMmrService;
        private readonly IValorantMatchService valorantMatchService;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateValorantPlayerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="valorantAccountService">Valorant Account Service, injected.</param>
        /// <param name="valorantMmrService">Valorant MMR Service, injected.</param>
        /// <param name="valorantMatchService">Valorant Match Service, injected.</param>
        /// <param name="mediator">MediatR sender, injected.</param>
        public UpdateValorantPlayerCommandHandler(
            IApplicationDbContext context,
            IValorantAccountService valorantAccountService,
            IValorantMmrService valorantMmrService,
            IValorantMatchService valorantMatchService,
            ISender mediator)
        {
            this.context = context;
            this.valorantAccountService = valorantAccountService;
            this.valorantMmrService = valorantMmrService;
            this.valorantMatchService = valorantMatchService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<PlayerDto> Handle(UpdateValorantPlayerCommand request, CancellationToken cancellationToken)
        {
            // Resolved by ID whenever the caller has one, and only otherwise by Keycloak ID: smurf
            // accounts have no Keycloak identity, and a null-matching lookup would happily refresh some
            // unrelated player that also has none.
            var playerInDb = request.Player.Id != 0
                ? await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.Player.Id, cancellationToken)
                : await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId != null && x.KeycloakId == request.Player.KeycloakId, cancellationToken);

            if (playerInDb == null)
            {
                throw new NotImplementedException();
            }

            ValorantAccountDto? valorantAccount = null;

            if (!string.IsNullOrWhiteSpace(playerInDb.ValorantPuuid))
            {
                valorantAccount = await this.valorantAccountService.GetAccountByPuuid(playerInDb.ValorantPuuid, cancellationToken);
            }
            else if (!string.IsNullOrWhiteSpace(playerInDb.RiotGamesNickname) && !string.IsNullOrWhiteSpace(playerInDb.RiotGamesTagLine))
            {
                valorantAccount = await this.valorantAccountService.GetAccountByRiotId(playerInDb.RiotGamesNickname, playerInDb.RiotGamesTagLine, cancellationToken);
            }

            if (valorantAccount is not null)
            {
                playerInDb.ValorantPuuid = valorantAccount.Puuid;
                playerInDb.ValorantAccountLevel = valorantAccount.AccountLevel;
                playerInDb.ValorantPlayerCardId = valorantAccount.Card;
                playerInDb.ValorantPlayerTitleId = valorantAccount.Title;
                playerInDb.ValorantRegion = valorantAccount.Region;
                playerInDb.ValorantRefreshedOn = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(valorantAccount.Name) && !string.IsNullOrWhiteSpace(valorantAccount.Tag))
                {
                    playerInDb.RiotGamesNickname = valorantAccount.Name;
                    playerInDb.RiotGamesTagLine = valorantAccount.Tag;
                }
            }

            this.context.Players.Update(playerInDb);
            await this.context.SaveChangesAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(playerInDb.ValorantPuuid))
            {
                var region = string.IsNullOrWhiteSpace(playerInDb.ValorantRegion) ? "eu" : playerInDb.ValorantRegion;
                var mmr = await this.valorantMmrService.GetMmrByPuuid(region, playerInDb.ValorantPuuid, cancellationToken);

                if (mmr?.Peak?.Tier is not null && mmr.Peak.Tier.Id >= 3)
                {
                    playerInDb.ValorantPeakTierId = mmr.Peak.Tier.Id;
                    playerInDb.ValorantPeakTierName = mmr.Peak.Tier.Name;
                    playerInDb.ValorantPeakRr = mmr.Peak.Rr;
                    playerInDb.ValorantPeakSeasonShort = mmr.Peak.Season?.Short;
                    playerInDb.ValorantPeakRankingSchema = mmr.Peak.RankingSchema;
                }

                if (mmr is not null)
                {
                    ApplyMmrStanding(playerInDb, mmr);
                    await this.SyncSeasonResults(playerInDb.Id, mmr, cancellationToken);
                }

                await this.SyncRankHistory(playerInDb.Id, region, playerInDb.ValorantPuuid, mmr, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);

                await this.ImportRecentMatches(region, playerInDb.ValorantPuuid, cancellationToken);
            }

            var dto = new PlayerDto(playerInDb);
            var playerRanks = await this.context.ValorantRankHistory
                .Where(x => x.PlayerId == playerInDb.Id)
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .ToListAsync(cancellationToken);

            dto.ValorantCurrentRank = playerRanks.FirstOrDefault();
            dto.RrChange7DaysValorant = ComputeRrChange7Days(playerRanks, DateTime.UtcNow.AddDays(-7));

            dto.RecentFormValorant = await this.context.ValorantGameParticipants
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

            dto.MainAgentName = mainAgent?.AgentName;
            dto.MainAgentIconUrl = string.IsNullOrWhiteSpace(mainAgent?.AgentId)
                ? null
                : $"https://media.valorant-api.com/agents/{mainAgent.AgentId}/displayicon.png";

            return dto;
        }

        private static int? ComputeRrChange7Days(List<ValorantRankHistory> historyDescending, DateTime sevenDaysAgo)
        {
            var current = historyDescending.FirstOrDefault(x => x.TierId >= 3 && x.Elo > 0);
            var baseline = historyDescending.FirstOrDefault(x => x.CreatedOn <= sevenDaysAgo && x.TierId >= 3 && x.Elo > 0);

            if (current is null || baseline is null)
            {
                return null;
            }

            return current.Elo - baseline.Elo;
        }

        private static void ApplyMmrStanding(Player player, ValorantMmrDto mmr)
        {
            var current = mmr.Current;

            if (current is not null)
            {
                player.ValorantRankProtectionShields = current.RankProtectionShields;
                player.ValorantRankProtectionStatus = current.RankProtectionStatus;
                player.ValorantIsAtRankProtectedTier = current.IsAtRankProtectedTier;
                player.ValorantGamesNeededForRating = current.GamesNeededForRating;
                player.ValorantGamesNeededForLeaderboard = current.GamesNeededForLeaderboard;
                player.ValorantLeaderboardRank = current.LeaderboardPlacement?.Rank;
                player.ValorantLeaderboardUpdatedOn = current.LeaderboardPlacement is null
                    ? null
                    : ValorantRankHistorySync.ParseUtcDate(current.LeaderboardPlacement.UpdatedAt);
            }

            player.ValorantIsActRankBadgeHidden = mmr.RankedState?.IsActRankBadgeHidden;
            player.ValorantIsLeaderboardAnonymized = mmr.RankedState?.IsLeaderboardAnonymized;
            player.ValorantLifetimePrestigeJson = mmr.LifetimePrestige is null || mmr.LifetimePrestige.Type == JTokenType.Null
                ? null
                : mmr.LifetimePrestige.ToString(Formatting.None);
        }

        private async Task SyncSeasonResults(int playerId, ValorantMmrDto mmr, CancellationToken cancellationToken)
        {
            var seasonal = mmr.Seasonal
                .Where(x => !string.IsNullOrWhiteSpace(x.Season?.Id))
                .GroupBy(x => x.Season!.Id, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.Last())
                .ToList();

            if (seasonal.Count == 0)
            {
                return;
            }

            var stored = await this.context.ValorantSeasonResults
                .Where(x => x.PlayerId == playerId)
                .ToListAsync(cancellationToken);

            var bySeasonId = stored.ToDictionary(x => x.SeasonId, StringComparer.OrdinalIgnoreCase);

            foreach (var act in seasonal)
            {
                if (!bySeasonId.TryGetValue(act.Season!.Id, out var row))
                {
                    row = new ValorantSeasonResult { PlayerId = playerId, SeasonId = act.Season.Id };
                    this.context.ValorantSeasonResults.Add(row);
                }

                row.SeasonShort = act.Season.Short;
                row.ActRankTierId = act.ActRank?.Id;
                row.ActRankTierName = act.ActRank?.Name;
                row.EndTierId = act.EndTier?.Id;
                row.EndTierName = act.EndTier?.Name;
                row.EndRr = act.EndRr;
                row.Games = act.Games;
                row.Wins = act.Wins;
                row.WinsWithPlacements = act.WinsWithPlacements;
                row.GamesNeededForRating = act.GamesNeededForRating;
                row.LeaderboardRank = act.LeaderboardPlacement?.Rank;
                row.RankingSchema = act.RankingSchema;
                row.ActWinsJson = JsonConvert.SerializeObject(act.ActWins.Select(x => x.Id));
                row.PrestigeJson = act.Prestige is null || act.Prestige.Type == JTokenType.Null ? null : act.Prestige.ToString(Formatting.None);
                row.RefreshedOn = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Imports the player's latest matches. When the whole first page is unknown, the refresh missed some
        /// (redeploy, quota refusal), so it keeps paging until it reaches a match already stored, up to
        /// <see cref="MaxGapPages"/> pages: holes get filled without waiting for a backfill.
        /// </summary>
        private async Task ImportRecentMatches(string region, string valorantPuuid, CancellationToken cancellationToken)
        {
            for (var page = 0; page < MaxGapPages; page++)
            {
                var matches = await this.valorantMatchService.GetLastMatchesByPuuid(
                    region,
                    valorantPuuid,
                    size: MatchPageSize,
                    start: page * MatchPageSize,
                    cancellationToken: cancellationToken);

                if (matches.Count == 0)
                {
                    return;
                }

                var matchIds = matches
                    .Select(x => x.Metadata?.MatchId)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x!)
                    .ToList();

                var knownCount = await this.context.ValorantGames.CountAsync(x => matchIds.Contains(x.MatchId), cancellationToken);

                await this.mediator.Send(new UpsertValorantMatchesCommand { Matches = matches }, cancellationToken);

                if (knownCount > 0 || matches.Count < MatchPageSize)
                {
                    return;
                }
            }
        }

        private async Task SyncRankHistory(int playerId, string region, string valorantPuuid, ValorantMmrDto? mmr, CancellationToken cancellationToken)
        {
            var mmrHistory = await this.valorantMmrService.GetMmrHistoryByPuuid(region, valorantPuuid, cancellationToken);

            var entries = await ValorantRankHistorySync.Upsert(
                this.context,
                playerId,
                mmrHistory?.History ?? new List<ValorantMmrHistoryEntryDto>(),
                cancellationToken);

            var newestEntry = entries.LastOrDefault();

            // If v3/mmr reports a current standing that differs from the newest entry we hold (e.g. a season
            // reset or an account whose match history rolled off), record a standalone snapshot.
            if (mmr?.Current?.Tier is not null)
            {
                var current = mmr.Current;
                var isSameStanding = newestEntry is not null
                    && newestEntry.TierId == current.Tier.Id
                    && newestEntry.RankedRating == current.Rr
                    && newestEntry.Elo == current.Elo;

                if (!isSameStanding && (current.Tier.Id >= 3 || newestEntry is null))
                {
                    this.context.ValorantRankHistory.Add(new ValorantRankHistory
                    {
                        PlayerId = playerId,
                        MatchId = null,
                        TierId = current.Tier.Id,
                        TierName = current.Tier.Name,
                        RankedRating = current.Rr,
                        Elo = current.Elo,
                        LastChange = current.LastChange,
                        CreatedOn = DateTime.UtcNow,
                    });
                }
            }
        }
    }
}