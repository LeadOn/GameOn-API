// <copyright file="GetLoLHomeStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Home.Queries.GetLoLHomeStats
{
    using GameOn.Application.LeagueOfLegends.Stats.Queries.GetLoLGlobalStats;
    using GameOn.Application.LeagueOfLegends.Summoners.Services;
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
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

        // "This week" / "last week" are cut on the players' wall clock (calendar weeks from Monday 00:00, or
        // the last seven days from midnight, see LoLHomeWindow), not on the UTC clock LoLGame.GameStart is
        // stored in (see GetLoLGlobalStatsQueryHandler).
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
            var nowUtc = DateTime.UtcNow;
            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, PlayersTimeZone);
            var todayLocal = nowLocal.Date;

            // First day of "this week" on the players' wall clock: Monday for the calendar week (the
            // historical behavior), six days ago for the rolling window so that today is its seventh day.
            // "Last week" is always the seven full days before it, whichever the window.
            var thisWeekStartLocal = request.Window == LoLHomeWindow.Last7Days
                ? todayLocal.AddDays(-6)
                : todayLocal.AddDays(-(((int)nowLocal.DayOfWeek + 6) % 7));
            var lastWeekStartLocal = thisWeekStartLocal.AddDays(-7);

            var thisWeekStart = TimeZoneInfo.ConvertTimeToUtc(thisWeekStartLocal, PlayersTimeZone);
            var lastWeekStart = TimeZoneInfo.ConvertTimeToUtc(lastWeekStartLocal, PlayersTimeZone);

            // Every day of this week, oldest first, today included. Days are cut on the players' wall clock,
            // so a day around a daylight saving change lasts 23 or 25 hours: each game and each snapshot is
            // placed by converting its own UTC timestamp (see ToPlayersDay), never by adding 24 hours.
            var days = Enumerable.Range(0, (todayLocal - thisWeekStartLocal).Days + 1)
                .Select(offset => DateOnly.FromDateTime(thisWeekStartLocal.AddDays(offset)))
                .ToList();

            // Every ranked game participation linked to a tracked account over the last two windows, with
            // just enough game context to compute the activity recap. Rows with an empty champion name are
            // placeholders left by failed imports, and remakes never count as played games. Which accounts
            // are tracked is decided by the two flags below, applied here and to the rank snapshots alike so
            // that every block of the page describes the same roster.
            var participantsQuery = this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.PlayerId != null
                    && x.ChampionName != string.Empty
                    && (x.Game.QueueId == RankedSoloQueueId || x.Game.QueueId == RankedFlexQueueId)
                    && !x.Game.IsRemake
                    && x.Game.GameStart >= lastWeekStart);

            if (!request.IncludeOutOfCrew)
            {
                participantsQuery = participantsQuery.Where(x => x.Player.InCrew);
            }

            if (!request.IncludeSmurfs)
            {
                participantsQuery = participantsQuery.Where(x => x.Player.PrimaryPlayerId == null);
            }

            // The real, loading-screen-free duration (LoLGameParticipantStat.GameDurationSeconds, derived
            // from the last timeline frame) is preferred, falling back to GameEnd - GameStart for the rare
            // game missing computed stats. Same fallback as the front's durationSecondsFor().
            var participants = (await participantsQuery
                .Select(x => new
                {
                    PlayerId = x.PlayerId!.Value,
                    x.Win,
                    x.Game.GameStart,
                    x.Game.GameEnd,
                    StatsGameDurationSeconds = x.Stats != null ? x.Stats.GameDurationSeconds : (int?)null,
                })
                .ToListAsync(cancellationToken))
                .Select(x => new
                {
                    x.PlayerId,
                    x.Win,
                    x.GameStart,
                    PlaytimeSeconds = x.StatsGameDurationSeconds is > 0
                        ? x.StatsGameDurationSeconds.Value
                        : Math.Max(0, (x.GameEnd - x.GameStart).TotalSeconds),
                })
                .ToList();

            var thisWeek = participants.Where(x => x.GameStart >= thisWeekStart).ToList();
            var lastWeek = participants.Where(x => x.GameStart < thisWeekStart).ToList();

            var wins = thisWeek.Count(x => x.Win);
            var games = thisWeek.Count;
            var winsLastWeek = lastWeek.Count(x => x.Win);

            var totalPlaytimeMinutes = thisWeek.Sum(x => x.PlaytimeSeconds) / 60.0;

            // Rank snapshots over the same two windows, per tracked account and per ranked queue
            // (Solo/Duo and Flex are tracked and compared separately, since their ladders are
            // independent). The same two flags as the participations above apply, so the net LP of the
            // week and the fact of the week both describe the same roster as the activity recap.
            var rankHistoryQuery = FilterRoster(
                this.context.LeagueOfLegendsRankHistory.Where(x => x.CreatedOn >= lastWeekStart),
                request);

            var rankHistory = await rankHistoryQuery
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

            // Day by day, each account and queue is compared against where it stood when the day began: the
            // last snapshot before the day, however old. Snapshots are only written when the rank moved, so
            // an account back after a month off starts its first day from its month-old snapshot, which is
            // loaded here separately since it falls outside the two windows loaded above. Restricted to the
            // accounts that actually moved this week, the only ones a day can be computed for.
            var snapshotsThisWeek = rankHistory.Where(x => x.CreatedOn >= thisWeekStart).ToList();
            var movedPlayerIds = snapshotsThisWeek.Select(x => x.PlayerId).Distinct().ToList();

            var snapshotsBeforeWeek = await FilterRoster(
                    this.context.LeagueOfLegendsRankHistory.Where(x => x.CreatedOn < thisWeekStart && movedPlayerIds.Contains(x.PlayerId)),
                    request)
                .GroupBy(x => new { x.PlayerId, x.QueueType })
                .Select(g => g
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => new { x.PlayerId, x.QueueType, x.Tier, x.Rank, x.LeaguePoints, x.CreatedOn })
                    .First())
                .ToListAsync(cancellationToken);

            var snapshotBeforeWeekByQueue = snapshotsBeforeWeek.ToDictionary(x => (x.PlayerId, x.QueueType));
            var netLpChangeByDay = new Dictionary<DateOnly, int>();

            foreach (var queueHistory in snapshotsThisWeek.GroupBy(x => (x.PlayerId, x.QueueType)))
            {
                var previous = snapshotBeforeWeekByQueue.GetValueOrDefault(queueHistory.Key);

                foreach (var lastOfDay in queueHistory
                    .GroupBy(x => ToPlayersDay(x.CreatedOn))
                    .OrderBy(x => x.Key)
                    .Select(x => x.OrderByDescending(snapshot => snapshot.CreatedOn).First()))
                {
                    if (previous is not null)
                    {
                        var before = LoLRankScaleCalculator.NormalizedLp(previous.Tier, previous.Rank, previous.LeaguePoints);
                        var after = LoLRankScaleCalculator.NormalizedLp(lastOfDay.Tier, lastOfDay.Rank, lastOfDay.LeaguePoints);

                        // A step with an end off the scale (placements, "UNRANKED") is dropped, same as
                        // the weekly comparison above does with its two ends.
                        if (before is not null && after is not null)
                        {
                            var day = ToPlayersDay(lastOfDay.CreatedOn);
                            netLpChangeByDay[day] = netLpChangeByDay.GetValueOrDefault(day) + (after.Value - before.Value);
                        }
                    }

                    previous = lastOfDay;
                }
            }

            // "Fact of the week": whoever gained the most LP this week, summed across their ranked queues.
            // Always the current week's top gainer, whether or not it beats any past record. Deterministic
            // tie-break on PlayerId, same convention as the global fun stats.
            int? topGainerId = lpChangeByPlayer.Count > 0
                ? lpChangeByPlayer.OrderByDescending(x => x.Value).ThenBy(x => x.Key).First().Key
                : null;

            // Most active first, then most wins, then player ID so that the order is stable from one refresh
            // to the next. The same games as GamesThisWeek, so the per-account counts add up to it.
            var activity = thisWeek
                .GroupBy(x => x.PlayerId)
                .Select(g => new { PlayerId = g.Key, Games = g.Count(), Wins = g.Count(x => x.Win) })
                .OrderByDescending(x => x.Games)
                .ThenByDescending(x => x.Wins)
                .ThenBy(x => x.PlayerId)
                .ToList();

            var playerIds = activity.Select(x => x.PlayerId).ToList();

            if (topGainerId is not null)
            {
                playerIds.Add(topGainerId.Value);
            }

            var players = await this.context.Players
                .Where(x => playerIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            LoLFactOfTheWeekDto? factOfTheWeek = null;

            if (topGainerId is not null && players.TryGetValue(topGainerId.Value, out var topGainerPlayer))
            {
                var topGainerGames = thisWeek.Where(x => x.PlayerId == topGainerId.Value).ToList();
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
                    LpChange = lpChangeByPlayer[topGainerId.Value],
                    GamesThisWeek = topGainerGames.Count,
                    WinsThisWeek = topGainerWins,
                    WinRateThisWeek = topGainerGames.Count > 0 ? Math.Round(100.0 * topGainerWins / topGainerGames.Count, 1) : 0,
                    LongestWinStreakThisWeek = longestWinStreak,
                };
            }

            var gamesByDay = thisWeek.ToLookup(x => ToPlayersDay(x.GameStart));
            var playtimeMinutesByDay = SpreadTenths(
                days.Select(day => gamesByDay[day].Sum(x => x.PlaytimeSeconds) / 60.0).ToList(),
                Math.Round(totalPlaytimeMinutes, 1));

            // "Records du crew": the same fun stat awards as GET lol/Stats/global, just scoped to the
            // ranked queues over a rolling month instead of recomputed here — the ranking/tie-break/
            // zero-data logic already lives in GetLoLGlobalStatsQueryHandler and shouldn't be duplicated.
            // Deliberately a wider window than the one used by the activity recap above: a single week
            // rarely holds enough ranked games for the awards to be meaningful.
            var crewRecords = await this.mediator.Send(
                new GetLoLGlobalStatsQuery
                {
                    RankedOnly = true,
                    Period = LoLStatsPeriod.Month,
                    IncludeSmurfs = request.IncludeSmurfs,
                    IncludeOutOfCrew = request.IncludeOutOfCrew,
                },
                cancellationToken);

            return new LoLHomeStatsDto
            {
                WeeklyActivity = new LoLWeeklyActivityDto
                {
                    WindowStart = thisWeekStart,
                    WindowEnd = nowUtc,
                    PreviousWindowStart = lastWeekStart,
                    GamesThisWeek = games,
                    GamesLastWeek = lastWeek.Count,
                    WinsThisWeek = wins,
                    LossesThisWeek = games - wins,
                    WinsLastWeek = winsLastWeek,
                    LossesLastWeek = lastWeek.Count - winsLastWeek,
                    WinRateThisWeek = games > 0 ? Math.Round(100.0 * wins / games, 1) : 0,
                    TotalPlaytimeMinutesThisWeek = Math.Round(totalPlaytimeMinutes, 1),
                    AverageGameDurationMinutesThisWeek = games > 0 ? Math.Round(totalPlaytimeMinutes / games, 1) : 0,
                    NetLpChangeThisWeek = netLpChangeThisWeek,
                    Days = days
                        .Select((day, index) => new LoLDailyActivityDto
                        {
                            Date = day,
                            Games = gamesByDay[day].Count(),
                            Wins = gamesByDay[day].Count(x => x.Win),
                            Losses = gamesByDay[day].Count(x => !x.Win),
                            PlaytimeMinutes = playtimeMinutesByDay[index],
                            NetLpChange = netLpChangeByDay.TryGetValue(day, out var lpChange) ? lpChange : null,
                        })
                        .ToList(),
                    ActivePlayers = activity
                        .Where(x => players.ContainsKey(x.PlayerId))
                        .Select(x => new LoLActivePlayerDto
                        {
                            Player = new PlayerDto(players[x.PlayerId]),
                            Games = x.Games,
                            Wins = x.Wins,
                        })
                        .ToList(),
                },
                FactOfTheWeek = factOfTheWeek,
                CrewRecords = crewRecords,
            };
        }

        /// <summary>
        /// The day a UTC timestamp falls on, on the players' wall clock.
        /// </summary>
        /// <param name="utc">Timestamp, in UTC.</param>
        /// <returns>Day, Europe/Paris.</returns>
        private static DateOnly ToPlayersDay(DateTime utc)
        {
            return DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), PlayersTimeZone));
        }

        /// <summary>
        /// Rounds each day's playtime to a tenth of a minute so that the days add up to the weekly total,
        /// itself rounded to a tenth. Rounding each day on its own could leave the sum a few tenths off the
        /// total; instead every day gets its rounded-down tenths, and the tenths left over go to the days
        /// that lost the most to rounding (largest remainder method, earliest day first on a tie).
        /// </summary>
        /// <param name="minutesPerDay">Exact playtime of each day, in minutes.</param>
        /// <param name="roundedTotalMinutes">Weekly total, already rounded to a tenth.</param>
        /// <returns>Each day's playtime, in minutes, rounded to a tenth.</returns>
        private static List<double> SpreadTenths(List<double> minutesPerDay, double roundedTotalMinutes)
        {
            var exactTenths = minutesPerDay.Select(x => x * 10).ToList();
            var tenths = exactTenths.Select(x => (long)Math.Floor(x)).ToList();
            var leftover = (long)Math.Round(roundedTotalMinutes * 10) - tenths.Sum();

            foreach (var index in Enumerable.Range(0, tenths.Count)
                .OrderByDescending(i => exactTenths[i] - tenths[i])
                .ThenBy(i => i)
                .Take((int)Math.Clamp(leftover, 0, tenths.Count)))
            {
                tenths[index]++;
            }

            return tenths.Select(x => x / 10.0).ToList();
        }

        /// <summary>
        /// Restricts rank snapshots to the roster the page describes, the same two flags as the game
        /// participations.
        /// </summary>
        /// <param name="rankHistory">Rank snapshots query.</param>
        /// <param name="request">The home page query, carrying the two roster flags.</param>
        /// <returns>The query, restricted to the requested roster.</returns>
        private static IQueryable<LeagueOfLegendsRankHistory> FilterRoster(IQueryable<LeagueOfLegendsRankHistory> rankHistory, GetLoLHomeStatsQuery request)
        {
            if (!request.IncludeOutOfCrew)
            {
                rankHistory = rankHistory.Where(x => x.Player.InCrew);
            }

            if (!request.IncludeSmurfs)
            {
                rankHistory = rankHistory.Where(x => x.Player.PrimaryPlayerId == null);
            }

            return rankHistory;
        }
    }
}
