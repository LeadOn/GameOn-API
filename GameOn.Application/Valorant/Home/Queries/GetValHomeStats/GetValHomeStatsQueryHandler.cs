// <copyright file="GetValHomeStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Home.Queries.GetValHomeStats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Services;
    using GameOn.Application.Valorant.Stats.Queries.GetValGlobalStats;
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetValHomeStatsQueryHandler class. Same layout and same window rules as <c>GetLoLHomeStatsQueryHandler</c>,
    /// on competitive games and RR instead of ranked games and LP.
    /// </summary>
    public class GetValHomeStatsQueryHandler : IRequestHandler<GetValHomeStatsQuery, ValHomeStatsDto>
    {
        // "This week" / "last week" are cut on the players' wall clock, not on the UTC clock games are stored in.
        private static readonly TimeZoneInfo PlayersTimeZone =
            TimeZoneInfo.TryFindSystemTimeZoneById("Europe/Paris", out var timeZone) ? timeZone : TimeZoneInfo.Utc;

        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValHomeStatsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">MediatR interface, injected.</param>
        public GetValHomeStatsQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<ValHomeStatsDto> Handle(GetValHomeStatsQuery request, CancellationToken cancellationToken)
        {
            var nowUtc = DateTime.UtcNow;
            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, PlayersTimeZone);
            var todayLocal = nowLocal.Date;

            var thisWeekStartLocal = request.Window == LoLHomeWindow.Last7Days
                ? todayLocal.AddDays(-6)
                : todayLocal.AddDays(-(((int)nowLocal.DayOfWeek + 6) % 7));
            var lastWeekStartLocal = thisWeekStartLocal.AddDays(-7);

            var thisWeekStart = TimeZoneInfo.ConvertTimeToUtc(thisWeekStartLocal, PlayersTimeZone);
            var lastWeekStart = TimeZoneInfo.ConvertTimeToUtc(lastWeekStartLocal, PlayersTimeZone);

            var days = Enumerable.Range(0, (todayLocal - thisWeekStartLocal).Days + 1)
                .Select(offset => DateOnly.FromDateTime(thisWeekStartLocal.AddDays(offset)))
                .ToList();

            var participantsQuery = this.context.ValorantGameParticipants
                .Where(x => x.PlayerId != null
                    && x.Game.QueueId == ValStatsQueueFilter.CompetitiveQueueId
                    && x.Game.GameStart >= lastWeekStart)
                .CountedGamesOnly();

            if (!request.IncludeOutOfCrew)
            {
                participantsQuery = participantsQuery.Where(x => x.Player!.InCrew);
            }

            if (!request.IncludeSmurfs)
            {
                participantsQuery = participantsQuery.Where(x => x.Player!.PrimaryPlayerId == null);
            }

            var participants = await participantsQuery.Select(ValPerformanceLine.Projection).ToListAsync(cancellationToken);

            var thisWeek = participants.Where(x => x.GameStart >= thisWeekStart).ToList();
            var lastWeek = participants.Where(x => x.GameStart < thisWeekStart).ToList();

            var games = thisWeek.Count;
            var wins = thisWeek.Count(x => x.Win);
            var winsLastWeek = lastWeek.Count(x => x.Win);
            var totalPlaytimeMinutes = thisWeek.Sum(x => x.GameLengthMs) / 60000.0;

            // Net RR is the sum of Riot's own per-match change, not a difference between two snapshots like
            // LoL's net LP: every ranked match is in the MMR history with the RR it moved, promotions and
            // demotions included, so there is nothing to rebuild. It also survives act resets, where the
            // displayed rank jumps without any game having been played. Rows without a match are the extra
            // snapshots written when the current rank differs from the history, and carry a copy of the last
            // match's change: summing them would count that match twice.
            var rankHistoryQuery = this.context.ValorantRankHistory
                .Where(x => x.MatchId != null && x.CreatedOn >= thisWeekStart);

            if (!request.IncludeOutOfCrew)
            {
                rankHistoryQuery = rankHistoryQuery.Where(x => x.Player.InCrew);
            }

            if (!request.IncludeSmurfs)
            {
                rankHistoryQuery = rankHistoryQuery.Where(x => x.Player.PrimaryPlayerId == null);
            }

            var rrChanges = await rankHistoryQuery
                .Select(x => new { x.PlayerId, x.LastChange, x.CreatedOn })
                .ToListAsync(cancellationToken);

            var rrChangeByPlayer = rrChanges
                .GroupBy(x => x.PlayerId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.LastChange));
            var rrChangesByDay = rrChanges.ToLookup(x => ToPlayersDay(x.CreatedOn));

            // Whoever gained the most RR this week. Deterministic tie-break on the player ID.
            int? topGainerId = rrChangeByPlayer.Count > 0
                ? rrChangeByPlayer.OrderByDescending(x => x.Value).ThenBy(x => x.Key).First().Key
                : null;

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

            ValFactOfTheWeekDto? factOfTheWeek = null;

            if (topGainerId is not null && players.TryGetValue(topGainerId.Value, out var topGainerPlayer))
            {
                var topGainerGames = thisWeek.Where(x => x.PlayerId == topGainerId.Value).OrderBy(x => x.GameStart).ToList();
                var topGainerWins = topGainerGames.Count(x => x.Win);

                var longestWinStreak = 0;
                var currentWinStreak = 0;

                foreach (var game in topGainerGames)
                {
                    currentWinStreak = game.Win ? currentWinStreak + 1 : 0;
                    longestWinStreak = Math.Max(longestWinStreak, currentWinStreak);
                }

                factOfTheWeek = new ValFactOfTheWeekDto
                {
                    Player = new PlayerDto(topGainerPlayer),
                    RrChange = rrChangeByPlayer[topGainerId.Value],
                    GamesThisWeek = topGainerGames.Count,
                    WinsThisWeek = topGainerWins,
                    WinRateThisWeek = ValPerformanceMath.WinRate(topGainerGames.Count, topGainerWins),
                    LongestWinStreakThisWeek = longestWinStreak,
                };
            }

            var gamesByDay = thisWeek.ToLookup(x => ToPlayersDay(x.GameStart));
            var playtimeMinutesByDay = SpreadTenths(
                days.Select(day => gamesByDay[day].Sum(x => x.GameLengthMs) / 60000.0).ToList(),
                Math.Round(totalPlaytimeMinutes, 1));

            // Same rolling month and same competitive-only scope as LoL's crew records: a single week rarely
            // holds enough games for the awards to mean anything.
            var crewRecords = await this.mediator.Send(
                new GetValGlobalStatsQuery
                {
                    RankedOnly = true,
                    Period = LoLStatsPeriod.Month,
                    IncludeSmurfs = request.IncludeSmurfs,
                    IncludeOutOfCrew = request.IncludeOutOfCrew,
                },
                cancellationToken);

            return new ValHomeStatsDto
            {
                WeeklyActivity = new ValWeeklyActivityDto
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
                    WinRateThisWeek = ValPerformanceMath.WinRate(games, wins),
                    TotalPlaytimeMinutesThisWeek = Math.Round(totalPlaytimeMinutes, 1),
                    AverageGameDurationMinutesThisWeek = games > 0 ? Math.Round(totalPlaytimeMinutes / games, 1) : 0,
                    NetRrChangeThisWeek = rrChanges.Sum(x => x.LastChange),
                    Days = days
                        .Select((day, index) => new ValDailyActivityDto
                        {
                            Date = day,
                            Games = gamesByDay[day].Count(),
                            Wins = gamesByDay[day].Count(x => x.Win),
                            Losses = gamesByDay[day].Count(x => !x.Win),
                            PlaytimeMinutes = playtimeMinutesByDay[index],
                            NetRrChange = rrChangesByDay[day].Any() ? rrChangesByDay[day].Sum(x => x.LastChange) : null,
                        })
                        .ToList(),
                    ActivePlayers = activity
                        .Where(x => players.ContainsKey(x.PlayerId))
                        .Select(x => new ValActivePlayerDto
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
        /// Rounds each day's playtime to a tenth of a minute so that the days add up to the weekly total, itself
        /// rounded to a tenth (largest remainder method, earliest day first on a tie). Same as
        /// <c>GetLoLHomeStatsQueryHandler.SpreadTenths</c>.
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
    }
}
