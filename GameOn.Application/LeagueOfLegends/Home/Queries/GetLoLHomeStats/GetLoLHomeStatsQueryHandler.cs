// <copyright file="GetLoLHomeStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Home.Queries.GetLoLHomeStats
{
    using GameOn.Application.LeagueOfLegends.Stats.Queries.GetLoLGlobalStats;
    using GameOn.Application.LeagueOfLegends.Summoners.Services;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLoLHomeStatsQueryHandler class.
    /// </summary>
    public class GetLoLHomeStatsQueryHandler : IRequestHandler<GetLoLHomeStatsQuery, LoLHomeStatsDto>
    {
        // Match-v5 queue ids of the two ranked ladders the home page reports on: Ranked Solo/Duo (420)
        // and Ranked Flex SR (440). An explicit whitelist rather than the keyword exclusion used by
        // GetLoLGlobalStatsQueryHandler: normals, ARAMs, bots, customs and tutorials are all out anyway.
        private const int RankedSoloQueueId = 420;
        private const int RankedFlexQueueId = 440;

        // "This week" / "last week" are calendar weeks (Monday 00:00 to Sunday 23:59:59) on the players'
        // wall clock, not the UTC clock LoLGame.GameStart is stored in (see GetLoLGlobalStatsQueryHandler).
        private static readonly TimeZoneInfo PlayersTimeZone =
            TimeZoneInfo.TryFindSystemTimeZoneById("Europe/Paris", out var timeZone) ? timeZone : TimeZoneInfo.Utc;

        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLoLHomeStatsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">MediatR interface, injected.</param>
        public GetLoLHomeStatsQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<LoLHomeStatsDto> Handle(GetLoLHomeStatsQuery request, CancellationToken cancellationToken)
        {
            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, PlayersTimeZone);
            var mondayThisWeekLocal = nowLocal.Date.AddDays(-(((int)nowLocal.DayOfWeek + 6) % 7));
            var mondayLastWeekLocal = mondayThisWeekLocal.AddDays(-7);

            var thisWeekStart = TimeZoneInfo.ConvertTimeToUtc(mondayThisWeekLocal, PlayersTimeZone);
            var lastWeekStart = TimeZoneInfo.ConvertTimeToUtc(mondayLastWeekLocal, PlayersTimeZone);

            // Every ranked game participation linked to a GameOn player over the last two calendar weeks,
            // with just enough game context to compute the activity recap. Rows with an empty champion
            // name are placeholders left by failed imports, and remakes never count as played games.
            var participants = await this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.PlayerId != null
                    && x.ChampionName != string.Empty
                    && (x.Game.QueueId == RankedSoloQueueId || x.Game.QueueId == RankedFlexQueueId)
                    && !x.Game.IsRemake
                    && x.Game.GameStart >= lastWeekStart)
                .Select(x => new
                {
                    PlayerId = x.PlayerId!.Value,
                    x.Win,
                    x.Game.GameStart,
                    x.Game.GameEnd,
                    StatsGameDurationSeconds = x.Stats != null ? x.Stats.GameDurationSeconds : (int?)null,
                })
                .ToListAsync(cancellationToken);

            var thisWeek = participants.Where(x => x.GameStart >= thisWeekStart).ToList();
            var lastWeek = participants.Where(x => x.GameStart < thisWeekStart).ToList();

            var wins = thisWeek.Count(x => x.Win);
            var games = thisWeek.Count;

            // The real, loading-screen-free duration (LoLGameParticipantStat.GameDurationSeconds, derived
            // from the last timeline frame) is preferred, falling back to GameEnd - GameStart for the rare
            // game missing computed stats. Same fallback as the front's durationSecondsFor().
            var totalPlaytimeMinutes = thisWeek.Sum(x => x.StatsGameDurationSeconds is > 0
                ? x.StatsGameDurationSeconds.Value
                : Math.Max(0, (x.GameEnd - x.GameStart).TotalSeconds)) / 60.0;

            // Rank snapshots over the same two calendar weeks, per player and per ranked queue (Solo/Duo
            // and Flex are tracked and compared separately, since their ladders are independent).
            var rankHistory = await this.context.LeagueOfLegendsRankHistory
                .Where(x => x.CreatedOn >= lastWeekStart)
                .Select(x => new { x.PlayerId, x.QueueType, x.Tier, x.Rank, x.LeaguePoints, x.CreatedOn })
                .ToListAsync(cancellationToken);

            var netLpChangeThisWeek = 0;
            var lpChangeByPlayer = new Dictionary<int, int>();

            foreach (var queueHistory in rankHistory.GroupBy(x => new { x.PlayerId, x.QueueType }))
            {
                // Only the last snapshot of each week is compared: intra-week LP wiggle (wins then losses)
                // isn't relevant, just where the player started and ended the week.
                var latestThisWeek = queueHistory.Where(x => x.CreatedOn >= thisWeekStart).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                var latestLastWeek = queueHistory.Where(x => x.CreatedOn < thisWeekStart).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                if (latestThisWeek is null || latestLastWeek is null)
                {
                    // No games/updates one of the two weeks: nothing to compare this queue against.
                    continue;
                }

                var thisWeekLp = LoLRankScaleCalculator.NormalizedLp(latestThisWeek.Tier, latestThisWeek.Rank, latestThisWeek.LeaguePoints);
                var lastWeekLp = LoLRankScaleCalculator.NormalizedLp(latestLastWeek.Tier, latestLastWeek.Rank, latestLastWeek.LeaguePoints);

                if (thisWeekLp is null || lastWeekLp is null)
                {
                    // Unrecognized tier/rank string (e.g. "UNRANKED"): not placeable on the scale.
                    continue;
                }

                var lpDelta = thisWeekLp.Value - lastWeekLp.Value;

                netLpChangeThisWeek += lpDelta;
                lpChangeByPlayer[queueHistory.Key.PlayerId] = lpChangeByPlayer.GetValueOrDefault(queueHistory.Key.PlayerId) + lpDelta;
            }

            // "Fact of the week": whoever gained the most LP this week, summed across their ranked queues.
            // Always the current week's top gainer, whether or not it beats any past record. Deterministic
            // tie-break on PlayerId, same convention as the global fun stats.
            LoLFactOfTheWeekDto? factOfTheWeek = null;

            if (lpChangeByPlayer.Count > 0)
            {
                var topGainer = lpChangeByPlayer.OrderByDescending(x => x.Value).ThenBy(x => x.Key).First();
                var topGainerPlayer = await this.context.Players.FirstOrDefaultAsync(x => x.Id == topGainer.Key, cancellationToken);

                if (topGainerPlayer is not null)
                {
                    var topGainerGames = thisWeek.Where(x => x.PlayerId == topGainer.Key).ToList();
                    var topGainerWins = topGainerGames.Count(x => x.Win);

                    var longestWinStreak = 0;
                    var currentWinStreak = 0;

                    foreach (var game in topGainerGames.OrderBy(x => x.GameStart))
                    {
                        if (!game.Win)
                        {
                            currentWinStreak = 0;
                            continue;
                        }

                        currentWinStreak++;
                        longestWinStreak = Math.Max(longestWinStreak, currentWinStreak);
                    }

                    factOfTheWeek = new LoLFactOfTheWeekDto
                    {
                        Player = topGainerPlayer,
                        LpChange = topGainer.Value,
                        GamesThisWeek = topGainerGames.Count,
                        WinsThisWeek = topGainerWins,
                        WinRateThisWeek = topGainerGames.Count > 0 ? Math.Round(100.0 * topGainerWins / topGainerGames.Count, 1) : 0,
                        LongestWinStreakThisWeek = longestWinStreak,
                    };
                }
            }

            // "Records du crew": the same fun stat awards as GET lol/Stats/global, just scoped to the
            // ranked queues over a rolling month instead of recomputed here — the ranking/tie-break/
            // zero-data logic already lives in GetLoLGlobalStatsQueryHandler and shouldn't be duplicated.
            // Deliberately a wider window than the calendar week used by the activity recap above: a
            // single week rarely holds enough ranked games for the awards to be meaningful.
            var crewRecords = await this.mediator.Send(
                new GetLoLGlobalStatsQuery { RankedOnly = true, Period = LoLStatsPeriod.Month },
                cancellationToken);

            return new LoLHomeStatsDto
            {
                WeeklyActivity = new LoLWeeklyActivityDto
                {
                    GamesThisWeek = games,
                    GamesLastWeek = lastWeek.Count,
                    WinsThisWeek = wins,
                    LossesThisWeek = games - wins,
                    WinRateThisWeek = games > 0 ? Math.Round(100.0 * wins / games, 1) : 0,
                    TotalPlaytimeMinutesThisWeek = Math.Round(totalPlaytimeMinutes, 1),
                    AverageGameDurationMinutesThisWeek = games > 0 ? Math.Round(totalPlaytimeMinutes / games, 1) : 0,
                    NetLpChangeThisWeek = netLpChangeThisWeek,
                },
                FactOfTheWeek = factOfTheWeek,
                CrewRecords = crewRecords,
            };
        }
    }
}
