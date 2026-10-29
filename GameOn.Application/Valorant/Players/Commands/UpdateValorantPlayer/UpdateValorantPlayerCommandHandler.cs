// <copyright file="UpdateValorantPlayerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Commands.UpsertValorantMatches;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateValorantPlayerCommandHandler class.
    /// </summary>
    public class UpdateValorantPlayerCommandHandler : IRequestHandler<UpdateValorantPlayerCommand, PlayerDto>
    {
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
                }

                await this.SyncRankHistory(playerInDb.Id, region, playerInDb.ValorantPuuid, mmr, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);

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

        private static DateTime ParseUtcDate(string? rawDate)
        {
            if (!string.IsNullOrWhiteSpace(rawDate)
                && DateTimeOffset.TryParse(rawDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var parsed))
            {
                return parsed.UtcDateTime;
            }

            return DateTime.UtcNow;
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

        private async Task SyncRankHistory(int playerId, string region, string valorantPuuid, ValorantMmrDto? mmr, CancellationToken cancellationToken)
        {
            var mmrHistory = await this.valorantMmrService.GetMmrHistoryByPuuid(region, valorantPuuid, cancellationToken);

            var existingEntries = await this.context.ValorantRankHistory
                .Where(x => x.PlayerId == playerId)
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .ToListAsync(cancellationToken);

            var knownMatchIds = new HashSet<string>(
                existingEntries.Where(x => !string.IsNullOrWhiteSpace(x.MatchId)).Select(x => x.MatchId!),
                StringComparer.OrdinalIgnoreCase);

            ValorantRankHistory? newestEntry = existingEntries.FirstOrDefault();

            if (mmrHistory?.History is { Count: > 0 })
            {
                // HenrikDev returns newest first; inserting oldest first keeps IDs ordered with CreatedOn.
                var chronologicalEntries = mmrHistory.History
                    .Select(entry => new { Entry = entry, ParsedDate = ParseUtcDate(entry.Date) })
                    .OrderBy(x => x.ParsedDate)
                    .ToList();

                foreach (var item in chronologicalEntries)
                {
                    var entry = item.Entry;

                    if (string.IsNullOrWhiteSpace(entry.MatchId) || !knownMatchIds.Add(entry.MatchId))
                    {
                        continue;
                    }

                    var rankEntry = new ValorantRankHistory
                    {
                        PlayerId = playerId,
                        MatchId = entry.MatchId,
                        SeasonId = entry.Season?.Id,
                        SeasonShort = entry.Season?.Short,
                        TierId = entry.Tier?.Id ?? 0,
                        TierName = entry.Tier?.Name ?? "Unranked",
                        RankedRating = entry.Rr,
                        Elo = entry.Elo,
                        LastChange = entry.LastChange,
                        RefundedRr = entry.RefundedRr,
                        WasDerankProtected = entry.WasDerankProtected,
                        MapId = entry.Map?.Id,
                        MapName = entry.Map?.Name,
                        CreatedOn = item.ParsedDate,
                    };

                    this.context.ValorantRankHistory.Add(rankEntry);

                    if (newestEntry is null || rankEntry.CreatedOn >= newestEntry.CreatedOn)
                    {
                        newestEntry = rankEntry;
                    }
                }
            }

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