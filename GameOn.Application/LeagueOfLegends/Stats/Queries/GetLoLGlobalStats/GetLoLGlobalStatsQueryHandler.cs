// <copyright file="GetLoLGlobalStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Stats.Queries.GetLoLGlobalStats
{
    using System.Globalization;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLoLGlobalStatsQueryHandler class.
    /// </summary>
    public class GetLoLGlobalStatsQueryHandler : IRequestHandler<GetLoLGlobalStatsQuery, LoLGlobalStatsDto>
    {
        private const int FifteenMinutesInMs = 900_000;
        private const int TwentyMinutesInMs = 1_200_000;
        private const int MinimumGamesForOneTrick = 10;
        private const int MinimumGamesForCursedPatch = 10;
        private const int ComebackGoldDeficit = 1000;
        private const int MinimumJungleMonstersForThief = 20;

        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLoLGlobalStatsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetLoLGlobalStatsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<LoLGlobalStatsDto> Handle(GetLoLGlobalStatsQuery request, CancellationToken cancellationToken)
        {
            // Every game participation linked to a GameOn player, with its game context.
            // Rows with an empty champion name are placeholders left by failed imports and are excluded.
            var participants = await this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.PlayerId != null && x.ChampionName != string.Empty)
                .Select(x => new
                {
                    PlayerId = x.PlayerId!.Value,
                    x.MatchId,
                    x.Puuid,
                    x.ChampionName,
                    x.Kills,
                    x.Deaths,
                    x.Assists,
                    Pings = x.AllInPings + x.AssistMePings + x.CommandPings,
                    x.BountyLevel,
                    x.ConsumablesPurchased,
                    x.Win,
                    x.TeamId,
                    x.Game.GameStart,
                    x.Game.GameVersion,
                })
                .ToListAsync(cancellationToken);

            var result = new LoLGlobalStatsDto
            {
                TotalGamesAnalyzed = participants.Select(x => x.MatchId).Distinct().Count(),
                TotalPlayersTracked = participants.Select(x => x.PlayerId).Distinct().Count(),
            };

            if (participants.Count == 0)
            {
                return result;
            }

            // Rank history snapshots, used for the LP drop award.
            var rankHistory = await this.context.LeagueOfLegendsRankHistory
                .Select(x => new { x.PlayerId, x.QueueType, x.Tier, x.Rank, x.LeaguePoints, x.CreatedOn })
                .ToListAsync(cancellationToken);

            // Last timeline frame of each game, for every participant (cumulative stats = end of game values).
            var lastFrames = await this.context.LeagueOfLegendsGameTimelineFrameParticipants
                .Where(x => x.TimelineFrame.Timestamp == x.TimelineFrame.Game.LoLGameTimelineFrames.Max(f => f.Timestamp))
                .Select(x => new
                {
                    x.TimelineFrame.MatchId,
                    x.TimelineFrame.Timestamp,
                    x.ParticipantId,
                    x.ParticipantPUUID,
                    x.CurrentGold,
                    x.TotalDamageTaken,
                    x.TotalDamageDoneToChampions,
                    x.TimeEnemySpentControlled,
                    x.JungleMinionsKilled,
                })
                .ToListAsync(cancellationToken);

            // First timeline frame at or after the 20 minutes mark, for the comeback award.
            var framesAt20 = await this.context.LeagueOfLegendsGameTimelineFrameParticipants
                .Where(x => x.TimelineFrame.Timestamp >= TwentyMinutesInMs
                    && x.TimelineFrame.Timestamp == x.TimelineFrame.Game.LoLGameTimelineFrames
                        .Where(f => f.Timestamp >= TwentyMinutesInMs)
                        .Min(f => f.Timestamp))
                .Select(x => new { x.TimelineFrame.MatchId, x.ParticipantId, x.TotalGold })
                .ToListAsync(cancellationToken);

            var playerIds = participants.Select(x => x.PlayerId)
                .Union(rankHistory.Select(x => x.PlayerId))
                .Distinct()
                .ToList();

            var players = await this.context.Players
                .Where(x => playerIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            var trackedLookup = participants.ToLookup(x => (x.MatchId, x.Puuid));

            // ------ Awards straight from game participations ------
            var topPinger = participants.OrderByDescending(x => x.Pings).First();
            result.PingMachine = BuildAward(players.GetValueOrDefault(topPinger.PlayerId), topPinger.Pings, $"{topPinger.Pings} pings on {topPinger.ChampionName}", topPinger.MatchId, topPinger.GameStart);

            var biggestInter = participants.OrderByDescending(x => x.Deaths).First();
            result.BiggestInter = BuildAward(players.GetValueOrDefault(biggestInter.PlayerId), biggestInter.Deaths, $"{biggestInter.Kills}/{biggestInter.Deaths}/{biggestInter.Assists} on {biggestInter.ChampionName}", biggestInter.MatchId, biggestInter.GameStart);

            var highestBounty = participants.OrderByDescending(x => x.BountyLevel).First();
            result.HighestBounty = BuildAward(players.GetValueOrDefault(highestBounty.PlayerId), highestBounty.BountyLevel, $"Bounty level {highestBounty.BountyLevel} on {highestBounty.ChampionName}", highestBounty.MatchId, highestBounty.GameStart);

            var shoppingAddict = participants.OrderByDescending(x => x.ConsumablesPurchased).First();
            result.ShoppingAddict = BuildAward(players.GetValueOrDefault(shoppingAddict.PlayerId), shoppingAddict.ConsumablesPurchased, $"{shoppingAddict.ConsumablesPurchased} consumables purchased on {shoppingAddict.ChampionName}", shoppingAddict.MatchId, shoppingAddict.GameStart);

            var oneTrick = participants
                .GroupBy(x => x.PlayerId)
                .Where(g => g.Count() >= MinimumGamesForOneTrick)
                .Select(g => new
                {
                    PlayerId = g.Key,
                    Total = g.Count(),
                    Favorite = g.GroupBy(x => x.ChampionName).OrderByDescending(c => c.Count()).First(),
                })
                .OrderByDescending(x => (double)x.Favorite.Count() / x.Total)
                .FirstOrDefault();

            if (oneTrick is not null)
            {
                var share = Math.Round(100.0 * oneTrick.Favorite.Count() / oneTrick.Total, 1);
                result.OneTrickPony = BuildAward(players.GetValueOrDefault(oneTrick.PlayerId), share, string.Create(CultureInfo.InvariantCulture, $"{oneTrick.Favorite.Key} played in {oneTrick.Favorite.Count()} of their {oneTrick.Total} games ({share}%)"), null, null);
            }

            // ------ Awards based on timeline frames ------
            var trackedFrames = lastFrames
                .Select(f => new { Frame = f, Tracked = trackedLookup[(f.MatchId, f.ParticipantPUUID)].FirstOrDefault() })
                .Where(x => x.Tracked is not null)
                .ToList();

            var ccMaster = trackedFrames.OrderByDescending(x => x.Frame.TimeEnemySpentControlled).FirstOrDefault();
            if (ccMaster is not null)
            {
                var ccSeconds = Math.Round(ccMaster.Frame.TimeEnemySpentControlled / 1000.0);
                result.CrowdControlMaster = BuildAward(players.GetValueOrDefault(ccMaster.Tracked!.PlayerId), ccSeconds, $"Enemies spent {ccSeconds} seconds controlled by {ccMaster.Tracked.ChampionName}", ccMaster.Frame.MatchId, ccMaster.Tracked.GameStart);
            }

            var punchingBall = trackedFrames
                .Where(x => x.Frame.Timestamp >= FifteenMinutesInMs)
                .OrderByDescending(x => x.Frame.TotalDamageTaken / (x.Frame.Timestamp / 60000.0))
                .FirstOrDefault();

            if (punchingBall is not null)
            {
                var damagePerMinute = Math.Round(punchingBall.Frame.TotalDamageTaken / (punchingBall.Frame.Timestamp / 60000.0));
                result.PunchingBall = BuildAward(players.GetValueOrDefault(punchingBall.Tracked!.PlayerId), damagePerMinute, $"{damagePerMinute} damage taken per minute on {punchingBall.Tracked.ChampionName}", punchingBall.Frame.MatchId, punchingBall.Tracked.GameStart);
            }

            var pacifist = trackedFrames
                .Where(x => x.Frame.Timestamp >= FifteenMinutesInMs)
                .OrderBy(x => x.Frame.TotalDamageDoneToChampions)
                .FirstOrDefault();

            if (pacifist is not null)
            {
                result.Pacifist = BuildAward(players.GetValueOrDefault(pacifist.Tracked!.PlayerId), pacifist.Frame.TotalDamageDoneToChampions, $"Only {pacifist.Frame.TotalDamageDoneToChampions} damage to champions on {pacifist.Tracked.ChampionName}", pacifist.Frame.MatchId, pacifist.Tracked.GameStart);
            }

            var squirrel = trackedFrames.OrderByDescending(x => x.Frame.CurrentGold).FirstOrDefault();
            if (squirrel is not null)
            {
                result.Squirrel = BuildAward(players.GetValueOrDefault(squirrel.Tracked!.PlayerId), squirrel.Frame.CurrentGold, $"Ended the game sitting on {squirrel.Frame.CurrentGold} unspent gold on {squirrel.Tracked.ChampionName}", squirrel.Frame.MatchId, squirrel.Tracked.GameStart);
            }

            var jungleThief = lastFrames
                .GroupBy(x => new { x.MatchId, Team = x.ParticipantId <= 5 ? 100 : 200 })
                .SelectMany(team =>
                {
                    var teamMax = team.Max(x => x.JungleMinionsKilled);
                    return team
                        .Where(x => x.JungleMinionsKilled < teamMax && x.JungleMinionsKilled >= MinimumJungleMonstersForThief)
                        .Select(x => new { Frame = x, Tracked = trackedLookup[(x.MatchId, x.ParticipantPUUID)].FirstOrDefault(), TeamMax = teamMax });
                })
                .Where(x => x.Tracked is not null)
                .OrderByDescending(x => x.Frame.JungleMinionsKilled)
                .FirstOrDefault();

            if (jungleThief is not null)
            {
                result.JungleThief = BuildAward(players.GetValueOrDefault(jungleThief.Tracked!.PlayerId), jungleThief.Frame.JungleMinionsKilled, $"{jungleThief.Frame.JungleMinionsKilled} jungle monsters on {jungleThief.Tracked.ChampionName} without being the jungler (jungler had {jungleThief.TeamMax})", jungleThief.Frame.MatchId, jungleThief.Tracked.GameStart);
            }

            var goldAt20ByMatch = framesAt20
                .GroupBy(x => x.MatchId)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Team100 = g.Where(x => x.ParticipantId <= 5).Sum(x => x.TotalGold),
                        Team200 = g.Where(x => x.ParticipantId > 5).Sum(x => x.TotalGold),
                    });

            var comebackKing = participants
                .Where(x => x.Win && goldAt20ByMatch.ContainsKey(x.MatchId))
                .Where(x =>
                {
                    var gold = goldAt20ByMatch[x.MatchId];
                    var own = x.TeamId == 100 ? gold.Team100 : gold.Team200;
                    var opponent = x.TeamId == 100 ? gold.Team200 : gold.Team100;
                    return opponent - own >= ComebackGoldDeficit;
                })
                .GroupBy(x => x.PlayerId)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (comebackKing is not null)
            {
                result.ComebackKing = BuildAward(players.GetValueOrDefault(comebackKing.Key), comebackKing.Count(), $"{comebackKing.Count()} wins after being at least {ComebackGoldDeficit} gold behind at 20 minutes", null, null);
            }

            // ------ Awards based on time and streaks ------
            var nightOwl = participants
                .GroupBy(x => x.PlayerId)
                .Select(g => new
                {
                    PlayerId = g.Key,
                    NightGames = g.Count(x => x.GameStart.Hour < 6),
                    NightWins = g.Count(x => x.GameStart.Hour < 6 && x.Win),
                })
                .Where(x => x.NightGames > 0)
                .OrderByDescending(x => x.NightGames)
                .FirstOrDefault();

            if (nightOwl is not null)
            {
                var nightWinRate = Math.Round(100.0 * nightOwl.NightWins / nightOwl.NightGames, 1);
                result.NightOwl = BuildAward(players.GetValueOrDefault(nightOwl.PlayerId), nightOwl.NightGames, string.Create(CultureInfo.InvariantCulture, $"{nightOwl.NightGames} games between midnight and 6 AM ({nightWinRate}% win rate)"), null, null);
            }

            var lossStreak = 0;
            var lossStreakPlayerId = 0;
            DateTime? lossStreakDate = null;

            foreach (var playerGames in participants.GroupBy(x => x.PlayerId))
            {
                var currentStreak = 0;

                foreach (var game in playerGames.OrderBy(x => x.GameStart))
                {
                    if (game.Win)
                    {
                        currentStreak = 0;
                        continue;
                    }

                    currentStreak++;

                    if (currentStreak > lossStreak)
                    {
                        lossStreak = currentStreak;
                        lossStreakPlayerId = playerGames.Key;
                        lossStreakDate = game.GameStart;
                    }
                }
            }

            if (lossStreak > 0)
            {
                result.LongestLossStreak = BuildAward(players.GetValueOrDefault(lossStreakPlayerId), lossStreak, $"{lossStreak} losses in a row", null, lossStreakDate);
            }

            var biggestDrop = 0;
            LoLFunStatDto? emotionalElevator = null;

            foreach (var queueHistory in rankHistory.GroupBy(x => new { x.PlayerId, x.QueueType }))
            {
                var snapshots = queueHistory.OrderBy(x => x.CreatedOn).ToList();

                for (var i = 1; i < snapshots.Count; i++)
                {
                    if (snapshots[i].Tier != snapshots[i - 1].Tier || snapshots[i].Rank != snapshots[i - 1].Rank)
                    {
                        continue;
                    }

                    var drop = snapshots[i - 1].LeaguePoints - snapshots[i].LeaguePoints;

                    if (drop > biggestDrop)
                    {
                        biggestDrop = drop;
                        emotionalElevator = BuildAward(players.GetValueOrDefault(queueHistory.Key.PlayerId), drop, $"-{drop} LP in {snapshots[i].Tier} {snapshots[i].Rank} ({queueHistory.Key.QueueType})", null, snapshots[i].CreatedOn);
                    }
                }
            }

            result.EmotionalElevator = emotionalElevator;

            var cursedPatch = participants
                .Where(x => !string.IsNullOrEmpty(x.GameVersion))
                .GroupBy(x => string.Join('.', x.GameVersion!.Split('.').Take(2)))
                .Where(g => g.Count() >= MinimumGamesForCursedPatch)
                .Select(g => new { Patch = g.Key, Games = g.Count(), Wins = g.Count(x => x.Win) })
                .OrderBy(x => (double)x.Wins / x.Games)
                .FirstOrDefault();

            if (cursedPatch is not null)
            {
                var winRate = Math.Round(100.0 * cursedPatch.Wins / cursedPatch.Games, 1);
                result.CursedPatch = BuildAward(null, winRate, string.Create(CultureInfo.InvariantCulture, $"Patch {cursedPatch.Patch}: {cursedPatch.Wins} wins out of {cursedPatch.Games} games ({winRate}% win rate)"), null, null);
            }

            return result;
        }

        private static LoLFunStatDto BuildAward(Player? player, double value, string? detail, string? matchId, DateTime? gameDate)
        {
            return new LoLFunStatDto
            {
                Player = player,
                Value = value,
                Detail = detail,
                MatchId = matchId,
                GameDate = gameDate,
            };
        }
    }
}
