// <copyright file="GetLoLHomeStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Home.Queries.GetLoLHomeStats
{
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
        // Matched against LoLQueue.Map + Description (synced from Riot) to keep only games against real
        // opponents. Duplicated from GetLoLGlobalStatsQueryHandler: worth factoring out into a shared
        // LoL query helper once a third Home stat block needs the same tracked-participant filter.
        private static readonly string[] ExcludedQueueTypeKeywords = { "Co-op", "Bot", "Tutorial", "Custom" };

        // "This week" / "last week" are calendar weeks (Monday 00:00 to Sunday 23:59:59) on the players'
        // wall clock, not the UTC clock LoLGame.GameStart is stored in (see GetLoLGlobalStatsQueryHandler).
        private static readonly TimeZoneInfo PlayersTimeZone =
            TimeZoneInfo.TryFindSystemTimeZoneById("Europe/Paris", out var timeZone) ? timeZone : TimeZoneInfo.Utc;

        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLoLHomeStatsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetLoLHomeStatsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<LoLHomeStatsDto> Handle(GetLoLHomeStatsQuery request, CancellationToken cancellationToken)
        {
            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, PlayersTimeZone);
            var mondayThisWeekLocal = nowLocal.Date.AddDays(-(((int)nowLocal.DayOfWeek + 6) % 7));
            var mondayLastWeekLocal = mondayThisWeekLocal.AddDays(-7);

            var thisWeekStart = TimeZoneInfo.ConvertTimeToUtc(mondayThisWeekLocal, PlayersTimeZone);
            var lastWeekStart = TimeZoneInfo.ConvertTimeToUtc(mondayLastWeekLocal, PlayersTimeZone);

            // Every game participation linked to a GameOn player over the last two calendar weeks, with
            // just enough game context to exclude remakes and bot/custom/tutorial queues below. Games
            // whose queue could not be resolved (LoLGame.Queue == null) are dropped entirely: without a
            // LoLQueue row there is no way to tell a real game from a bot or custom one.
            var participants = (await this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.PlayerId != null
                    && x.ChampionName != string.Empty
                    && x.Game.Queue != null
                    && !x.Game.IsRemake
                    && x.Game.GameStart >= lastWeekStart)
                .Select(x => new
                {
                    x.Win,
                    x.Game.GameStart,
                    x.Game.GameEnd,
                    QueueMap = x.Game.Queue!.Map,
                    QueueDescription = x.Game.Queue!.Description,
                    StatsGameDurationSeconds = x.Stats != null ? x.Stats.GameDurationSeconds : (int?)null,
                })
                .ToListAsync(cancellationToken))
                .Where(x => !ExcludedQueueTypeKeywords.Any(keyword =>
                    ((x.QueueMap ?? string.Empty) + " " + (x.QueueDescription ?? string.Empty))
                        .Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                .ToList();

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

                netLpChangeThisWeek += thisWeekLp.Value - lastWeekLp.Value;
            }

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
            };
        }
    }
}
