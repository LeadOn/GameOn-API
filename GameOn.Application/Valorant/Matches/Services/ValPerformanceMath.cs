// <copyright file="ValPerformanceMath.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameOn.Common.DTOs.Valorant;

    /// <summary>
    /// Aggregates <see cref="ValPerformanceLine"/>s into the rates the Valorant stats expose. Every rate is
    /// computed on totals (score of every round over the number of rounds) rather than averaged game by game,
    /// so that a 24-round game weighs more than a 9-round Swiftplay. Per-round rates only read the lines that
    /// have derived stats, since the round count lives there.
    /// </summary>
    public static class ValPerformanceMath
    {
        /// <summary>
        /// Win rate, as a percentage rounded to a tenth. 0 without games.
        /// </summary>
        /// <param name="games">Number of games.</param>
        /// <param name="wins">Number of wins.</param>
        /// <returns>Win rate.</returns>
        public static double WinRate(int games, int wins)
        {
            return games > 0 ? Math.Round(100.0 * wins / games, 1) : 0;
        }

        /// <summary>
        /// KDA on totals: <c>(kills + assists) / max(deaths, 1)</c>.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>KDA, rounded to a hundredth.</returns>
        public static double Kda(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            return Math.Round((double)(lines.Sum(x => x.Kills) + lines.Sum(x => x.Assists)) / Math.Max(lines.Sum(x => x.Deaths), 1), 2);
        }

        /// <summary>
        /// K/D on totals: <c>kills / max(deaths, 1)</c>.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>K/D, rounded to a hundredth.</returns>
        public static double KdRatio(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            return Math.Round((double)lines.Sum(x => x.Kills) / Math.Max(lines.Sum(x => x.Deaths), 1), 2);
        }

        /// <summary>
        /// Average Combat Score on totals. 0 when no line has rounds.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>ACS, rounded to a tenth.</returns>
        public static double Acs(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            return PerRound(lines, x => x.Score);
        }

        /// <summary>
        /// Average Damage per Round on totals. 0 when no line has rounds.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>ADR, rounded to a tenth.</returns>
        public static double Adr(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            return PerRound(lines, x => x.DamageDealt);
        }

        /// <summary>
        /// Headshot percentage over every shot that hit. 0 when no shot hit.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>Headshot percentage, rounded to a tenth.</returns>
        public static double HeadshotPercent(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            var shots = lines.Sum(x => x.ShotsHit);

            return shots > 0 ? Math.Round(100.0 * lines.Sum(x => x.Headshots) / shots, 1) : 0;
        }

        /// <summary>
        /// KAST percentage, each game weighted by its rounds. 0 when no line has rounds.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>KAST percentage, rounded to a tenth.</returns>
        public static double KastPercent(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            var withRounds = lines.Where(x => x.HasStats && x.RoundsPlayed > 0).ToList();
            var rounds = withRounds.Sum(x => x.RoundsPlayed);

            return rounds > 0 ? Math.Round(withRounds.Sum(x => x.KastPercent * x.RoundsPlayed) / rounds, 1) : 0;
        }

        /// <summary>
        /// Breakdown of the agents played, most played first (ties broken by win rate, then agent name).
        /// Lines without an agent are left out.
        /// </summary>
        /// <typeparam name="TStat">The agent stat type to build (the crew-wide one adds a top player).</typeparam>
        /// <param name="lines">Participations.</param>
        /// <returns>One entry per agent, with the lines it was built from.</returns>
        public static List<(TStat Stat, List<ValPerformanceLine> Lines)> AgentStats<TStat>(IEnumerable<ValPerformanceLine> lines)
            where TStat : ValAgentStatDto, new()
        {
            return lines
                .Where(x => !string.IsNullOrWhiteSpace(x.AgentName))
                .GroupBy(x => x.AgentName!)
                .Select(g =>
                {
                    var agentLines = g.ToList();
                    var agentId = agentLines.Select(x => x.AgentId).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
                    var wins = agentLines.Count(x => x.Win);

                    var stat = new TStat
                    {
                        AgentId = agentId,
                        AgentName = g.Key,
                        AgentIconUrl = agentId is null ? null : $"https://media.valorant-api.com/agents/{agentId}/displayicon.png",
                        GamesPlayed = agentLines.Count,
                        Wins = wins,
                        WinRate = WinRate(agentLines.Count, wins),
                        Kda = Kda(agentLines),
                        Acs = Acs(agentLines),
                        HeadshotPercent = HeadshotPercent(agentLines),
                    };

                    return (Stat: stat, Lines: agentLines);
                })
                .OrderByDescending(x => x.Stat.GamesPlayed)
                .ThenByDescending(x => x.Stat.WinRate)
                .ThenBy(x => x.Stat.AgentName, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// Breakdown of the maps played, most played first (ties broken by win rate, then map name). A game
        /// counts once per team, so that several tracked accounts in the same team weigh as one game; the ACS
        /// still reads every participation. Lines without a map are left out.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>One entry per map.</returns>
        public static List<ValMapStatDto> MapStats(IEnumerable<ValPerformanceLine> lines)
        {
            return lines
                .Where(x => !string.IsNullOrWhiteSpace(x.MapName))
                .GroupBy(x => x.MapName!)
                .Select(g =>
                {
                    var mapLines = g.ToList();
                    var teamGames = mapLines.GroupBy(x => (x.MatchId, x.TeamId)).Select(t => t.First()).ToList();
                    var wins = teamGames.Count(x => x.Win);
                    var mapId = mapLines.Select(x => x.MapId).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

                    return new ValMapStatDto
                    {
                        MapId = mapId,
                        MapName = g.Key,
                        MapListViewIconUrl = mapId is null ? null : $"https://media.valorant-api.com/maps/{mapId}/listviewicon.png",
                        GamesPlayed = teamGames.Count,
                        Wins = wins,
                        WinRate = WinRate(teamGames.Count, wins),
                        Acs = Acs(mapLines),
                    };
                })
                .OrderByDescending(x => x.GamesPlayed)
                .ThenByDescending(x => x.WinRate)
                .ThenBy(x => x.MapName, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// Attack and defense figures, summed over the games whose sides are resolved.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>Attack and defense, both null when no game has its sides resolved.</returns>
        public static (ValSideStatsDto? Attack, ValSideStatsDto? Defense) Sides(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            var sided = lines.Where(x => x.AttackRounds is not null && x.DefenseRounds is not null).ToList();

            if (sided.Count == 0)
            {
                return (null, null);
            }

            static ValSideStatsDto Side(int rounds, int won, int kills, int deaths, int score) => new ValSideStatsDto
            {
                Rounds = rounds,
                RoundsWon = won,
                RoundWinRatePercent = WinRate(rounds, won),
                Kills = kills,
                Deaths = deaths,
                KdRatio = Math.Round(kills / (double)Math.Max(deaths, 1), 2),
                Acs = rounds > 0 ? Math.Round((double)score / rounds, 1) : 0,
            };

            return (
                Side(sided.Sum(x => x.AttackRounds!.Value), sided.Sum(x => x.AttackRoundsWon ?? 0), sided.Sum(x => x.AttackKills ?? 0), sided.Sum(x => x.AttackDeaths ?? 0), sided.Sum(x => x.AttackScore ?? 0)),
                Side(sided.Sum(x => x.DefenseRounds!.Value), sided.Sum(x => x.DefenseRoundsWon ?? 0), sided.Sum(x => x.DefenseKills ?? 0), sided.Sum(x => x.DefenseDeaths ?? 0), sided.Sum(x => x.DefenseScore ?? 0)));
        }

        /// <summary>
        /// Rounds by buy type, summed over the standard-mode games.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>Rounds by buy type, null without any standard-mode game.</returns>
        public static ValBuyTypeStatsDto? BuyTypes(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            var bought = lines.Where(x => x.PistolRounds is not null).ToList();

            if (bought.Count == 0)
            {
                return null;
            }

            var pistol = bought.Sum(x => x.PistolRounds ?? 0);
            var pistolWon = bought.Sum(x => x.PistolRoundsWon ?? 0);
            var eco = bought.Sum(x => x.EcoRounds ?? 0);
            var ecoWon = bought.Sum(x => x.EcoRoundsWon ?? 0);
            var force = bought.Sum(x => x.ForceBuyRounds ?? 0);
            var forceWon = bought.Sum(x => x.ForceBuyRoundsWon ?? 0);
            var full = bought.Sum(x => x.FullBuyRounds ?? 0);
            var fullWon = bought.Sum(x => x.FullBuyRoundsWon ?? 0);

            return new ValBuyTypeStatsDto
            {
                PistolRounds = pistol,
                PistolRoundsWon = pistolWon,
                PistolWinRatePercent = WinRate(pistol, pistolWon),
                EcoRounds = eco,
                EcoRoundsWon = ecoWon,
                EcoWinRatePercent = WinRate(eco, ecoWon),
                ForceBuyRounds = force,
                ForceBuyRoundsWon = forceWon,
                ForceBuyWinRatePercent = WinRate(force, forceWon),
                FullBuyRounds = full,
                FullBuyRoundsWon = fullWon,
                FullBuyWinRatePercent = WinRate(full, fullWon),
            };
        }

        /// <summary>
        /// Clutches, summed over the games.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>Clutches.</returns>
        public static ValClutchStatsDto Clutches(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            var won1v1 = lines.Sum(x => x.Clutch1v1Won);
            var won1v2 = lines.Sum(x => x.Clutch1v2Won);
            var won1v3 = lines.Sum(x => x.Clutch1v3Won);
            var won1v4 = lines.Sum(x => x.Clutch1v4Won);
            var won1v5 = lines.Sum(x => x.Clutch1v5Won);
            var won = won1v1 + won1v2 + won1v3 + won1v4 + won1v5;
            var attempts = lines.Sum(x => x.ClutchAttempts);

            return new ValClutchStatsDto
            {
                Attempts = attempts,
                Won = won,
                WinRatePercent = WinRate(attempts, won),
                Won1v1 = won1v1,
                Won1v2 = won1v2,
                Won1v3 = won1v3,
                Won1v4 = won1v4,
                Won1v5 = won1v5,
            };
        }

        /// <summary>
        /// Trades, summed over the games with round stats.
        /// </summary>
        /// <param name="lines">Participations.</param>
        /// <returns>Trades.</returns>
        public static ValTradeStatsDto Trades(IReadOnlyCollection<ValPerformanceLine> lines)
        {
            var withStats = lines.Where(x => x.HasStats).ToList();
            var deaths = withStats.Sum(x => x.Deaths);
            var traded = withStats.Sum(x => x.TradedDeaths);

            return new ValTradeStatsDto
            {
                TradeKills = withStats.Sum(x => x.TradeKills),
                TradedDeaths = traded,
                Deaths = deaths,
                TradedDeathPercent = deaths > 0 ? Math.Round(100.0 * traded / deaths, 1) : 0,
            };
        }

        private static double PerRound(IReadOnlyCollection<ValPerformanceLine> lines, Func<ValPerformanceLine, int> value)
        {
            var withRounds = lines.Where(x => x.HasStats && x.RoundsPlayed > 0).ToList();
            var rounds = withRounds.Sum(x => x.RoundsPlayed);

            return rounds > 0 ? Math.Round((double)withRounds.Sum(value) / rounds, 1) : 0;
        }
    }
}
