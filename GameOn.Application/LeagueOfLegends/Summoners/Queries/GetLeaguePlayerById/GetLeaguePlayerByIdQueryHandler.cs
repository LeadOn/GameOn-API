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

        // Match-v5 queue IDs for the two ranked queues (see GetAllLeaguePlayersQueryHandler, which uses
        // the same constants). league-v4's RANKED_SOLO_5x5 / RANKED_FLEX_SR QueueType strings, used for
        // the rank snapshots below, don't line up with these — the two Riot APIs don't share identifiers.
        private const int SoloQueueId = 420;
        private const int FlexQueueId = 440;

        private const string SoloQueueType = "RANKED_SOLO_5x5";
        private const string FlexQueueType = "RANKED_FLEX_SR";

        // The profile page's rank cards show 8 form squares, wider than the 5 used by the compact ladder
        // row in GetAllLeaguePlayersQueryHandler — same data, different display, so a different count here
        // is intentional rather than a drift to reconcile.
        private const int RecentFormGameCount = 8;

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
                var soloRank = await this.context.LeagueOfLegendsRankHistory.OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync(x => x.PlayerId == playerInDb.Id && x.QueueType == SoloQueueType, cancellationToken);

                playerInDb.LeagueOfLegendsSoloRank = soloRank;

                var flexRank = await this.context.LeagueOfLegendsRankHistory.OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync(x => x.PlayerId == playerInDb.Id && x.QueueType == FlexQueueType, cancellationToken);

                playerInDb.LeagueOfLegendsFlexRank = flexRank;

                // Informational only: the other accounts this player owns, listed with their own rank.
                // Nothing below aggregates them — every stat on this profile is that of the requested
                // account alone.
                playerInDb.SmurfAccounts = await this.GetSmurfAccounts(playerInDb.Id, cancellationToken);

                // Newest first coming out of the query, and kept that way: the front renders the form
                // squares most-recent first, so no side re-orders the series. Remakes and empty-champion
                // placeholders (failed imports) are excluded, same filter as
                // GetAllLeaguePlayersQueryHandler / GetLoLGlobalStatsQueryHandler.
                var recentRankedGames = await this.context.LeagueOfLegendsGameParticipants
                    .Where(x => x.PlayerId == playerInDb.Id
                        && x.ChampionName != string.Empty
                        && !x.Game.IsRemake
                        && (x.Game.QueueId == SoloQueueId || x.Game.QueueId == FlexQueueId))
                    .OrderByDescending(x => x.Game.GameStart)
                    .Select(x => new { x.Game.QueueId, x.Win })
                    .ToListAsync(cancellationToken);

                playerInDb.RecentFormSolo = recentRankedGames.Where(x => x.QueueId == SoloQueueId).Take(RecentFormGameCount).Select(x => x.Win).ToList();
                playerInDb.RecentFormFlex = recentRankedGames.Where(x => x.QueueId == FlexQueueId).Take(RecentFormGameCount).Select(x => x.Win).ToList();

                playerInDb.PerformanceStats = await this.GetPerformanceStats(playerInDb.Id, request.Period, request.QueueIds, request.TeamPosition, cancellationToken);
            }

            return playerInDb;
        }

        /// <summary>
        /// The smurf accounts attached to a player, each carrying its own current Solo/Duo and Flex rank.
        /// Empty for a player with no smurf, and for a smurf account itself (nesting is one level deep).
        /// </summary>
        /// <param name="playerId">Primary account ID.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>The player's smurf accounts, oldest first.</returns>
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

            var smurfAccountIds = smurfAccounts.Select(x => x.Id).ToList();

            // Newest first, so the first match per account and queue below is that account's current rank.
            var ranks = await this.context.LeagueOfLegendsRankHistory
                .Where(x => smurfAccountIds.Contains(x.PlayerId) && (x.QueueType == SoloQueueType || x.QueueType == FlexQueueType))
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync(cancellationToken);

            foreach (var smurfAccount in smurfAccounts)
            {
                smurfAccount.LeagueOfLegendsSoloRank = ranks.FirstOrDefault(x => x.PlayerId == smurfAccount.Id && x.QueueType == SoloQueueType);
                smurfAccount.LeagueOfLegendsFlexRank = ranks.FirstOrDefault(x => x.PlayerId == smurfAccount.Id && x.QueueType == FlexQueueType);
            }

            return smurfAccounts;
        }

        private async Task<LoLSummonerPerformanceStatsDto?> GetPerformanceStats(int playerId, LoLStatsPeriod period, List<int>? queueIds, string? teamPosition, CancellationToken cancellationToken)
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

            if (queueIds is { Count: > 0 })
            {
                query = query.Where(x => x.Game.QueueId.HasValue && queueIds.Contains(x.Game.QueueId.Value));
            }

            // Riot writes the position in upper case (TOP, JUNGLE, ...), so the caller's value is
            // normalized rather than trusted as-is. Games Riot could not assign a role to keep an empty
            // TeamPosition and are therefore excluded by this filter, as intended.
            var normalizedTeamPosition = NormalizeTeamPosition(teamPosition);

            if (normalizedTeamPosition is not null)
            {
                query = query.Where(x => x.TeamPosition == normalizedTeamPosition);
            }

            var games = (await query
                .Select(x => new
                {
                    x.MatchId,
                    x.TeamId,
                    x.Win,
                    x.Game.GameStart,
                    x.Game.GameEnd,
                    x.ChampionName,
                    x.TeamPosition,
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

            // Only games with a role Riot could resolve count towards the play rate: older imports
            // (before the Phase 1 Riot-data capture) and lane-less game modes leave TeamPosition empty,
            // and would otherwise silently deflate every role's share.
            var gamesWithRole = games.Where(x => !string.IsNullOrEmpty(x.TeamPosition)).ToList();

            // Team and result of this player in each of their (already filtered) games, to tell teammates
            // from opponents below and to credit duo wins without re-deriving them from scratch.
            var matchInfoByMatchId = games.ToDictionary(x => x.MatchId, x => new { x.TeamId, x.Win });
            var matchIds = matchInfoByMatchId.Keys.ToList();

            // Every other GameOn-tracked participant in those same matches. Bots/unlinked players
            // (PlayerId null) can't be credited as a duo, and the player themselves is excluded. Duos are
            // per account: playing with someone's main and with their smurf counts as two partners, which
            // is what they were on the Rift.
            var teammateParticipations = await this.context.LeagueOfLegendsGameParticipants
                .Where(x => matchIds.Contains(x.MatchId) && x.PlayerId != null && x.PlayerId.Value != playerId)
                .Select(x => new { x.MatchId, x.TeamId, PlayerId = x.PlayerId!.Value })
                .ToListAsync(cancellationToken);

            var duoStats = teammateParticipations
                .Where(x => matchInfoByMatchId[x.MatchId].TeamId == x.TeamId)
                .GroupBy(x => x.PlayerId)
                .Select(g => new
                {
                    PlayerId = g.Key,
                    GamesPlayed = g.Count(),
                    Wins = g.Count(x => matchInfoByMatchId[x.MatchId].Win),
                })
                .ToList();

            var teammateIds = duoStats.Select(x => x.PlayerId).ToList();
            var teammates = await this.context.Players
                .Where(x => teammateIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

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
                ChampionStats = games
                    .GroupBy(x => x.ChampionName)
                    .Select(g => new LoLChampionStatDto
                    {
                        ChampionName = g.Key,
                        GamesPlayed = g.Count(),
                        Wins = g.Count(x => x.Win),
                        WinRate = Math.Round(100.0 * g.Count(x => x.Win) / g.Count(), 1),
                        Kda = Math.Round(g.Select(x => x.Kda).Where(x => x is not null).Select(x => x!.Value).DefaultIfEmpty(0).Average(), 2),
                    })
                    .OrderByDescending(x => x.GamesPlayed)
                    .ThenByDescending(x => x.WinRate)
                    .ThenBy(x => x.ChampionName, StringComparer.Ordinal)
                    .ToList(),
                RoleStats = gamesWithRole
                    .GroupBy(x => x.TeamPosition)
                    .Select(g => new LoLRoleStatDto
                    {
                        TeamPosition = g.Key,
                        GamesPlayed = g.Count(),
                        Wins = g.Count(x => x.Win),
                        PlayRate = Math.Round(100.0 * g.Count() / gamesWithRole.Count, 1),
                        WinRate = Math.Round(100.0 * g.Count(x => x.Win) / g.Count(), 1),
                    })
                    .OrderByDescending(x => x.PlayRate)
                    .ThenByDescending(x => x.WinRate)
                    .ThenBy(x => x.TeamPosition, StringComparer.Ordinal)
                    .ToList(),
                DuoStats = duoStats
                    .Where(x => teammates.ContainsKey(x.PlayerId))
                    .Select(x => new LoLDuoStatDto
                    {
                        Player = teammates[x.PlayerId],
                        GamesPlayed = x.GamesPlayed,
                        Wins = x.Wins,
                        WinRate = Math.Round(100.0 * x.Wins / x.GamesPlayed, 1),
                    })
                    .OrderByDescending(x => x.GamesPlayed)
                    .ThenByDescending(x => x.WinRate)
                    .ThenBy(x => x.Player.Id)
                    .ToList(),
            };
        }

        /// <summary>
        /// Normalizes a caller-supplied team position to the upper-case form Riot stores.
        /// Duplicated from <see cref="Matches.Queries.GetLastGamesPlayed.GetLastGamesPlayedQueryHandler"/>:
        /// worth factoring out into a shared helper if a third caller shows up.
        /// </summary>
        /// <param name="teamPosition">Raw team position coming from the query.</param>
        /// <returns>Normalized team position, or null when no filter was asked for.</returns>
        private static string? NormalizeTeamPosition(string? teamPosition)
        {
            return string.IsNullOrWhiteSpace(teamPosition) ? null : teamPosition.Trim().ToUpperInvariant();
        }
    }
}
