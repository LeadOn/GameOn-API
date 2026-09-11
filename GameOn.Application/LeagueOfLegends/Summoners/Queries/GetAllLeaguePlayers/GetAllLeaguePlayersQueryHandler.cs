// <copyright file="GetAllLeaguePlayersQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetAllLeaguePlayers
{
    using GameOn.Application.Common.Players;
    using GameOn.Application.LeagueOfLegends.Summoners.Services;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllLeaguePlayersQueryHandler class.
    /// </summary>
    public class GetAllLeaguePlayersQueryHandler : IRequestHandler<GetAllLeaguePlayersQuery, IEnumerable<PlayerDto>>
    {
        // Match-v5 queue IDs for the two ranked queues (see GetLoLGlobalStatsQueryHandler, which uses the
        // same constants). league-v4's RANKED_SOLO_5x5 / RANKED_FLEX_SR QueueType strings, used for the
        // rank history below, don't line up with these — the two Riot APIs don't share identifiers.
        private const int SoloQueueId = 420;
        private const int FlexQueueId = 440;

        private const string SoloQueueType = "RANKED_SOLO_5x5";
        private const string FlexQueueType = "RANKED_FLEX_SR";

        private const int RecentFormGameCount = 5;

        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLeaguePlayersQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllLeaguePlayersQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PlayerDto>> Handle(GetAllLeaguePlayersQuery request, CancellationToken cancellationToken)
        {
            // One entry per account, smurfs included: rank, LP delta and recent form are per account by
            // nature — a smurf sits on its own ladder — so merging them into the owner's card would be
            // meaningless. Each entry carries PrimaryPlayerId, which is what a caller needs to nest a
            // smurf under its owner; IncludeSmurfs = false drops them for callers that want members only.
            var playersQuery = this.context.Players.Include(x => x.TournamentsWon).AsQueryable();

            if (!request.IncludeSmurfs)
            {
                playersQuery = playersQuery.PrimariesOnly();
            }

            var playersInDb = await playersQuery.Where(x => x.Archived == request.Archived && x.RiotGamesPUUID != null).Select(x => new PlayerDto(x)).ToListAsync(cancellationToken);

            var playerIds = playersInDb.Select(x => x.Id).ToList();

            // Every rank snapshot (league-v4's sparse change log, see UpdatePlayerSummonerCommandHandler:
            // a row is only inserted when tier/rank/LP actually changed) for the tracked players and
            // ranked queues, newest first so both the current rank and the 7-day baseline can be read off
            // the same in-memory list per player/queue below.
            var rankHistory = await this.context.LeagueOfLegendsRankHistory
                .Where(x => playerIds.Contains(x.PlayerId) && (x.QueueType == SoloQueueType || x.QueueType == FlexQueueType))
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync(cancellationToken);

            // Every ranked Solo/Duo or Flex participation for the tracked players, newest first, just
            // enough to read off the last 5 results per player/queue below. Remakes and empty-champion
            // placeholders (failed imports) are excluded, same filter as GetLoLGlobalStatsQueryHandler.
            var recentGames = await this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.PlayerId != null
                    && playerIds.Contains(x.PlayerId.Value)
                    && x.ChampionName != string.Empty
                    && !x.Game.IsRemake
                    && (x.Game.QueueId == SoloQueueId || x.Game.QueueId == FlexQueueId))
                .OrderByDescending(x => x.Game.GameStart)
                .Select(x => new { x.PlayerId, x.Game.QueueId, x.Win })
                .ToListAsync(cancellationToken);

            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            foreach (var player in playersInDb)
            {
                var soloHistory = rankHistory.Where(x => x.PlayerId == player.Id && x.QueueType == SoloQueueType).ToList();
                var flexHistory = rankHistory.Where(x => x.PlayerId == player.Id && x.QueueType == FlexQueueType).ToList();

                player.LeagueOfLegendsSoloRank = soloHistory.FirstOrDefault();
                player.LeagueOfLegendsFlexRank = flexHistory.FirstOrDefault();

                player.LpChange7DaysSolo = GetLpChange7Days(soloHistory, sevenDaysAgo);
                player.LpChange7DaysFlex = GetLpChange7Days(flexHistory, sevenDaysAgo);

                // Newest first coming out of the query above, and kept that way: the front renders the
                // form squares most-recent first, so no side re-orders the series.
                player.RecentFormSolo = recentGames
                    .Where(x => x.PlayerId == player.Id && x.QueueId == SoloQueueId)
                    .Take(RecentFormGameCount)
                    .Select(x => x.Win)
                    .ToList();

                player.RecentFormFlex = recentGames
                    .Where(x => x.PlayerId == player.Id && x.QueueId == FlexQueueId)
                    .Take(RecentFormGameCount)
                    .Select(x => x.Win)
                    .ToList();
            }

            return playersInDb;
        }

        /// <summary>
        /// Compares the most recent rank snapshot against the last one at or before <paramref name="sevenDaysAgo"/>.
        /// </summary>
        /// <param name="queueHistoryDescending">A single queue's rank history for one player, newest first.</param>
        /// <param name="sevenDaysAgo">The 7-day-ago cutoff, in the same clock as <see cref="LeagueOfLegendsRankHistory.CreatedOn"/> (UTC).</param>
        /// <returns>The LP change, or null when there's no snapshot on one side of the window or the tier/rank isn't placeable on the scale.</returns>
        private static int? GetLpChange7Days(List<LeagueOfLegendsRankHistory> queueHistoryDescending, DateTime sevenDaysAgo)
        {
            var current = queueHistoryDescending.FirstOrDefault();
            var baseline = queueHistoryDescending.FirstOrDefault(x => x.CreatedOn <= sevenDaysAgo);

            if (current is null || baseline is null)
            {
                return null;
            }

            var currentLp = LoLRankScaleCalculator.NormalizedLp(current.Tier, current.Rank, current.LeaguePoints);
            var baselineLp = LoLRankScaleCalculator.NormalizedLp(baseline.Tier, baseline.Rank, baseline.LeaguePoints);

            if (currentLp is null || baselineLp is null)
            {
                return null;
            }

            return currentLp.Value - baselineLp.Value;
        }
    }
}
