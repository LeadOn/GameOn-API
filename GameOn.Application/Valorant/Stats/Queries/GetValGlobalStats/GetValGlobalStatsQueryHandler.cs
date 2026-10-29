// <copyright file="GetValGlobalStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Stats.Queries.GetValGlobalStats
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
    /// GetValGlobalStatsQueryHandler class.
    /// </summary>
    public class GetValGlobalStatsQueryHandler : IRequestHandler<GetValGlobalStatsQuery, ValGlobalStatsDto>
    {
        // Below this many games, a headshot percentage is a coin toss: one lucky Sheriff game would win it.
        private const int MinimumGamesForAccuracy = 5;

        // Same reason as LoL's Night Owl: GameStart is stored in UTC, the award reasons on the players' clock.
        private static readonly TimeZoneInfo PlayersTimeZone =
            TimeZoneInfo.TryFindSystemTimeZoneById("Europe/Paris", out var timeZone) ? timeZone : TimeZoneInfo.Utc;

        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValGlobalStatsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValGlobalStatsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<ValGlobalStatsDto> Handle(GetValGlobalStatsQuery request, CancellationToken cancellationToken)
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
                .Where(x => x.PlayerId != null)
                .CountedGamesOnly();

            // Same roster rules as GET lol/Stats/global: crew only unless asked otherwise, and smurfs are
            // accounts of their own, dropped rather than merged when excluded.
            if (!request.IncludeOutOfCrew)
            {
                query = query.Where(x => x.Player!.InCrew);
            }

            if (!request.IncludeSmurfs)
            {
                query = query.Where(x => x.Player!.PrimaryPlayerId == null);
            }

            if (request.RankedOnly)
            {
                query = query.Where(x => x.Game.QueueId == ValStatsQueueFilter.CompetitiveQueueId);
            }

            if (since is not null)
            {
                query = query.Where(x => x.Game.GameStart >= since);
            }

            var lines = await query.Select(ValPerformanceLine.Projection).ToListAsync(cancellationToken);

            var playerIds = lines.Select(x => x.PlayerId).Distinct().ToList();
            var players = await this.context.Players
                .Where(x => playerIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            var topAgents = ValPerformanceMath.AgentStats<ValCrewAgentStatDto>(lines).Take(5).ToList();

            foreach (var (stat, agentLines) in topAgents)
            {
                // Who played it the most, within the very same games. Ties go to the one who won more on it,
                // then to the lowest ID, so that the pick doesn't flip between refreshes.
                var top = agentLines
                    .GroupBy(x => x.PlayerId)
                    .Select(p => new { PlayerId = p.Key, GamesPlayed = p.Count(), Wins = p.Count(x => x.Win) })
                    .OrderByDescending(p => p.GamesPlayed)
                    .ThenByDescending(p => p.Wins)
                    .ThenBy(p => p.PlayerId)
                    .First();

                stat.TopPlayer = new ValTopPlayerDto
                {
                    Player = new PlayerDto(players[top.PlayerId]),
                    GamesPlayed = top.GamesPlayed,
                };
            }

            var result = new ValGlobalStatsDto
            {
                TotalGamesAnalyzed = lines.Select(x => x.MatchId).Distinct().Count(),
                TotalPlayersTracked = playerIds.Count,
                TopAgents = topAgents.Select(x => x.Stat).ToList(),
                TopMaps = ValPerformanceMath.MapStats(lines).Take(5).ToList(),
            };

            if (lines.Count == 0)
            {
                return result;
            }

            // ------ Single-game awards ------
            // Each ranks on the very value it displays, ties go to the match ID for a stable pick, and the
            // award stays null when the best value is zero: a record of zero is missing data, not a record.
            var highestCombatScore = lines
                .Where(x => x.HasStats && x.RoundsPlayed > 0)
                .OrderByDescending(x => x.Acs)
                .ThenBy(x => x.MatchId, StringComparer.Ordinal)
                .FirstOrDefault();

            if (highestCombatScore is { Acs: > 0 })
            {
                var acs = Math.Round(highestCombatScore.Acs, 1);
                result.HighestCombatScore = BuildAward(players, highestCombatScore, acs, string.Create(CultureInfo.InvariantCulture, $"{acs} ACS on {AgentOf(highestCombatScore)} ({ScoreLine(highestCombatScore)})"));
            }

            var entryKing = lines
                .OrderByDescending(x => x.FirstKills)
                .ThenBy(x => x.FirstDeaths)
                .ThenBy(x => x.MatchId, StringComparer.Ordinal)
                .First();

            if (entryKing.FirstKills > 0)
            {
                result.EntryKing = BuildAward(players, entryKing, entryKing.FirstKills, $"{entryKing.FirstKills} first kills on {AgentOf(entryKing)} ({entryKing.FirstDeaths} first deaths)");
            }

            var firstBloodVictim = lines
                .OrderByDescending(x => x.FirstDeaths)
                .ThenBy(x => x.FirstKills)
                .ThenBy(x => x.MatchId, StringComparer.Ordinal)
                .First();

            if (firstBloodVictim.FirstDeaths > 0)
            {
                result.FirstBloodVictim = BuildAward(players, firstBloodVictim, firstBloodVictim.FirstDeaths, $"First to die in {firstBloodVictim.FirstDeaths} rounds on {AgentOf(firstBloodVictim)}");
            }

            // Same reading as LoL: dying a lot is not inting if the player contributed, but that correction
            // only breaks ties, so the award displays its own ranking key and stays monotonic across periods.
            var biggestInter = lines
                .OrderByDescending(x => x.Deaths)
                .ThenByDescending(x => x.Deaths - x.Kills - (x.Assists / 2.0))
                .ThenBy(x => x.MatchId, StringComparer.Ordinal)
                .First();

            if (biggestInter.Deaths > 0)
            {
                result.BiggestInter = BuildAward(players, biggestInter, biggestInter.Deaths, $"{ScoreLine(biggestInter)} on {AgentOf(biggestInter)}");
            }

            var spikeCarrier = lines
                .OrderByDescending(x => x.SpikePlants + x.SpikeDefuses)
                .ThenBy(x => x.MatchId, StringComparer.Ordinal)
                .First();

            if (spikeCarrier.SpikePlants + spikeCarrier.SpikeDefuses > 0)
            {
                result.SpikeCarrier = BuildAward(players, spikeCarrier, spikeCarrier.SpikePlants + spikeCarrier.SpikeDefuses, $"{spikeCarrier.SpikePlants} plants and {spikeCarrier.SpikeDefuses} defuses on {AgentOf(spikeCarrier)}");
            }

            // ------ Awards over the whole period ------
            var accuracy = lines
                .GroupBy(x => x.PlayerId)
                .Select(g => new
                {
                    PlayerId = g.Key,
                    Games = g.Count(),
                    Headshots = g.Sum(x => x.Headshots),
                    Shots = g.Sum(x => x.ShotsHit),
                })
                .Where(x => x.Games >= MinimumGamesForAccuracy && x.Shots > 0)
                .Select(x => new { x.PlayerId, x.Games, x.Headshots, x.Shots, Percent = Math.Round(100.0 * x.Headshots / x.Shots, 1) })
                .ToList();

            var sharpshooter = accuracy.OrderByDescending(x => x.Percent).ThenBy(x => x.PlayerId).FirstOrDefault();

            if (sharpshooter is { Percent: > 0 })
            {
                result.Sharpshooter = BuildAward(players, sharpshooter.PlayerId, sharpshooter.Percent, string.Create(CultureInfo.InvariantCulture, $"{sharpshooter.Percent}% headshots ({sharpshooter.Headshots} of {sharpshooter.Shots} shots hit over {sharpshooter.Games} games)"), null);
            }

            // With a single eligible account, it would be crowned best and worst shot at once.
            if (accuracy.Count >= 2)
            {
                var bodyshotEnjoyer = accuracy.OrderBy(x => x.Percent).ThenBy(x => x.PlayerId).First();
                result.BodyshotEnjoyer = BuildAward(players, bodyshotEnjoyer.PlayerId, bodyshotEnjoyer.Percent, string.Create(CultureInfo.InvariantCulture, $"Only {bodyshotEnjoyer.Percent}% headshots ({bodyshotEnjoyer.Headshots} of {bodyshotEnjoyer.Shots} shots hit over {bodyshotEnjoyer.Games} games)"), null);
            }

            var knifeAssassin = lines
                .GroupBy(x => x.PlayerId)
                .Select(g => new { PlayerId = g.Key, KnifeKills = g.Sum(x => x.KnifeKills), Games = g.Count() })
                .OrderByDescending(x => x.KnifeKills)
                .ThenBy(x => x.PlayerId)
                .First();

            if (knifeAssassin.KnifeKills > 0)
            {
                result.KnifeAssassin = BuildAward(players, knifeAssassin.PlayerId, knifeAssassin.KnifeKills, $"{knifeAssassin.KnifeKills} knife kills over {knifeAssassin.Games} games", null);
            }

            var nightOwl = lines
                .GroupBy(x => x.PlayerId)
                .Select(g => new
                {
                    PlayerId = g.Key,
                    NightGames = g.Count(x => IsNightGame(x.GameStart)),
                    NightWins = g.Count(x => IsNightGame(x.GameStart) && x.Win),
                })
                .Where(x => x.NightGames > 0)
                .OrderByDescending(x => x.NightGames)
                .ThenBy(x => x.PlayerId)
                .FirstOrDefault();

            if (nightOwl is not null)
            {
                var nightWinRate = ValPerformanceMath.WinRate(nightOwl.NightGames, nightOwl.NightWins);
                result.NightOwl = BuildAward(players, nightOwl.PlayerId, nightOwl.NightGames, string.Create(CultureInfo.InvariantCulture, $"{nightOwl.NightGames} games between midnight and 6 AM ({nightWinRate}% win rate)"), null);
            }

            result.LongestWinStreak = BuildStreakAward(players, lines, x => x.Win, "wins");

            // A draw is not a loss, so it breaks a losing streak like a win does.
            result.LongestLossStreak = BuildStreakAward(players, lines, x => !x.Win && !x.IsDraw, "losses");

            return result;
        }

        /// <summary>
        /// Tells whether a game was started between midnight and 6 AM on the players' wall clock.
        /// </summary>
        /// <param name="gameStart">Game start, in UTC.</param>
        /// <returns>True when the game started during the night.</returns>
        private static bool IsNightGame(DateTime gameStart)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(gameStart, DateTimeKind.Utc), PlayersTimeZone).Hour < 6;
        }

        private static string AgentOf(ValPerformanceLine line)
        {
            return string.IsNullOrWhiteSpace(line.AgentName) ? "an unknown agent" : line.AgentName;
        }

        private static string ScoreLine(ValPerformanceLine line)
        {
            return $"{line.Kills}/{line.Deaths}/{line.Assists}";
        }

        /// <summary>
        /// Longest run of consecutive games matching <paramref name="extends"/>, any account. Ties go to the
        /// lowest player ID. Null when no game matches at all.
        /// </summary>
        /// <param name="players">Accounts, by ID.</param>
        /// <param name="lines">Participations.</param>
        /// <param name="extends">Whether a game extends the streak; any other game resets it.</param>
        /// <param name="noun">Plural noun of the streak, for the detail.</param>
        /// <returns>The award, or null.</returns>
        private static ValFunStatDto? BuildStreakAward(Dictionary<int, Player> players, List<ValPerformanceLine> lines, Func<ValPerformanceLine, bool> extends, string noun)
        {
            var best = 0;
            var bestPlayerId = 0;
            DateTime? bestDate = null;

            foreach (var playerGames in lines.GroupBy(x => x.PlayerId).OrderBy(x => x.Key))
            {
                var current = 0;

                foreach (var game in playerGames.OrderBy(x => x.GameStart))
                {
                    current = extends(game) ? current + 1 : 0;

                    if (current > best)
                    {
                        best = current;
                        bestPlayerId = playerGames.Key;
                        bestDate = game.GameStart;
                    }
                }
            }

            if (best == 0)
            {
                return null;
            }

            var award = BuildAward(players, bestPlayerId, best, $"{best} {noun} in a row", null);
            award.GameDate = bestDate;

            return award;
        }

        private static ValFunStatDto BuildAward(Dictionary<int, Player> players, ValPerformanceLine line, double value, string detail)
        {
            var award = BuildAward(players, line.PlayerId, value, detail, line.MatchId);
            award.GameDate = line.GameStart;

            return award;
        }

        private static ValFunStatDto BuildAward(Dictionary<int, Player> players, int playerId, double value, string detail, string? matchId)
        {
            return new ValFunStatDto
            {
                Player = players.TryGetValue(playerId, out var player) ? new PlayerDto(player) : null,
                Value = value,
                Detail = detail,
                MatchId = matchId,
            };
        }
    }
}
