// <copyright file="GetPlayerStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Players.Queries.GetPlayerStats
{
    using MediatR;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Application.FifaGamePlayed.Queries.GetFifaGamePlayedById;
    using YuGames.Application.FifaTeams.Queries.GetMostLossesFifaTeamsByPlayer;
    using YuGames.Application.FifaTeams.Queries.GetMostPlayedFifaTeamsByPlayer;
    using YuGames.Application.FifaTeams.Queries.GetMostWinsFifaTeamsByPlayer;
    using YuGames.Application.Platforms.Queries.GetAllPlatforms;
    using YuGames.Application.Players.Queries.GetPlayerById;
    using YuGames.Application.TeamPlayers.Queries.SearchTeamPlayer;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

    /// <summary>
    /// GetAllPlayersQueryHandler class.
    /// </summary>
    public class GetPlayerStatsQueryHandler : IRequestHandler<GetPlayerStatsQuery, FifaPlayerStatsDto?>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPlayerStatsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetPlayerStatsQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<FifaPlayerStatsDto?> Handle(GetPlayerStatsQuery request, CancellationToken cancellationToken)
        {
            request.SeasonId ??= int.Parse(Environment.GetEnvironmentVariable("CURRENT_SEASON") ?? "1");

            // Get player from database.
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = request.PlayerId }, cancellationToken);

            if (playerInDb is null)
            {
                return null;
            }

            // Getting stats per platforms
            var platformsStats = new List<PlatformStatsDto>();

            var platformsInDb = await this.mediator.Send(new GetAllPlatformsQuery(), cancellationToken);

            var globalStats = new PlatformStatsDto();
            globalStats.Platform = new Platform { Id = 0, Name = "Global" };
            var avgGoalsTaken = new List<float>();
            var avgGoalsGiven = new List<float>();

            foreach (var platform in platformsInDb)
            {
                var stats = new PlatformStatsDto { AverageGoalGiven = 0, AverageGoalTaken = 0, Draws = 0, GoalDifference = 0, Losses = 0, Platform = platform, Wins = 0, GoalsGiven = 0, GoalsTaken = 0 };

                // Getting games played by platform
                var teamPlayersInDb = await this.mediator.Send(
                    new SearchTeamPlayerQuery
                {
                    Query = x => x.FifaGamePlayed.PlatformId == platform.Id
                                 && x.PlayerId == playerInDb.Id
                                 && x.FifaGamePlayed.SeasonId == request.SeasonId
                                 && x.FifaGamePlayed.IsPlayed == true,
                    Limit = 1000000,
                }, cancellationToken);

                if (teamPlayersInDb.ToList().Count > 0)
                {
                    // For each game played, getting that stats
                    foreach (var teamPlayer in teamPlayersInDb)
                    {
                        // Getting game
                        var gameInDb = await this.mediator.Send(new GetFifaGamePlayedByIdQuery { FifaGamePlayedId = teamPlayer.FifaGameId }, cancellationToken);

                        if (gameInDb is null)
                        {
                            throw new NotImplementedException();
                        }

                        if (gameInDb.Team1.Score == gameInDb.Team2.Score)
                        {
                            stats.Draws++;
                        }
                        else if ((teamPlayer.Team == 0 && gameInDb.Team1.Score > gameInDb.Team2.Score) || (teamPlayer.Team == 1 && gameInDb.Team1.Score < gameInDb.Team2.Score))
                        {
                            stats.Wins++;
                        }
                        else if ((teamPlayer.Team == 0 && gameInDb.Team1.Score < gameInDb.Team2.Score) || (teamPlayer.Team == 1 && gameInDb.Team1.Score > gameInDb.Team2.Score))
                        {
                            stats.Losses++;
                        }

                        if (teamPlayer.Team == 0)
                        {
                            stats.GoalsGiven += gameInDb.Team1.Score;
                            stats.GoalsTaken += gameInDb.Team2.Score;
                        }
                        else if (teamPlayer.Team == 1)
                        {
                            stats.GoalsGiven += gameInDb.Team2.Score;
                            stats.GoalsTaken += gameInDb.Team1.Score;
                        }
                    }

                    stats.GoalDifference = stats.GoalsGiven - stats.GoalsTaken;
                    if (stats.GoalsGiven == 0 || (stats.Wins + stats.Losses + stats.Draws) == 0)
                    {
                        stats.AverageGoalGiven = 0;
                    }
                    else
                    {
                        stats.AverageGoalGiven = (float)Math.Round((double)(stats.GoalsGiven / (float)(stats.Wins + stats.Draws + stats.Losses)), 2);
                    }

                    if (stats.GoalsTaken == 0 || (stats.Wins + stats.Losses + stats.Draws) == 0)
                    {
                        stats.AverageGoalTaken = 0;
                    }
                    else
                    {
                        stats.AverageGoalTaken = (float)Math.Round((double)(stats.GoalsTaken / (float)(stats.Wins + stats.Draws + stats.Losses)), 2);
                    }

                    var gamesPlayed = stats.Wins + stats.Losses + stats.Draws;

                    // Calculating win rate
                    if (stats.Wins == 0 || gamesPlayed == 0)
                    {
                        stats.WinRate = 0;
                    }
                    else
                    {
                        stats.WinRate = (float)Math.Round((double)((stats.Wins * 100) / (float)gamesPlayed), 2);
                    }

                    // Calculating loose rate
                    if (stats.Losses == 0 || gamesPlayed == 0)
                    {
                        stats.LooseRate = 0;
                    }
                    else
                    {
                        stats.LooseRate = (float)Math.Round((double)((stats.Losses * 100) / (float)gamesPlayed), 2);
                    }

                    // Calculating draw rate
                    if (stats.Draws == 0 || gamesPlayed == 0)
                    {
                        stats.DrawRate = 0;
                    }
                    else
                    {
                        stats.DrawRate = (float)Math.Round((double)((stats.Draws * 100) / (float)gamesPlayed), 2);
                    }

                    globalStats.Wins += stats.Wins;
                    globalStats.Losses += stats.Losses;
                    globalStats.Draws += stats.Draws;
                    globalStats.GoalDifference += stats.GoalDifference;
                    globalStats.GoalsTaken += stats.GoalsTaken;
                    globalStats.GoalsGiven += stats.GoalsGiven;
                    avgGoalsGiven.Add(stats.AverageGoalGiven);
                    avgGoalsTaken.Add(stats.AverageGoalTaken);

                    platformsStats.Add(stats);
                }
            }

            if (avgGoalsGiven.Count != 0)
            {
                globalStats.AverageGoalGiven = (float)Math.Round((double)avgGoalsGiven.Average(), 2);
            }

            if (avgGoalsTaken.Count != 0)
            {
                globalStats.AverageGoalTaken = (float)Math.Round((double)avgGoalsTaken.Average(), 2);
            }

            var totalGamesPlayed = globalStats.Wins + globalStats.Losses + globalStats.Draws;

            // Calculating win rate
            if (globalStats.Wins == 0 || totalGamesPlayed == 0)
            {
                globalStats.WinRate = 0;
            }
            else
            {
                globalStats.WinRate = (float)Math.Round((double)((globalStats.Wins * 100) / (float)totalGamesPlayed), 2);
            }

            // Calculating loose rate
            if (globalStats.Losses == 0 || totalGamesPlayed == 0)
            {
                globalStats.LooseRate = 0;
            }
            else
            {
                globalStats.LooseRate = (float)Math.Round((double)((globalStats.Losses * 100) / (float)totalGamesPlayed), 2);
            }

            // Calculating draw rate
            if (globalStats.Draws == 0 || totalGamesPlayed == 0)
            {
                globalStats.DrawRate = 0;
            }
            else
            {
                globalStats.DrawRate = (float)Math.Round((double)((globalStats.Draws * 100) / (float)totalGamesPlayed), 2);
            }

            platformsStats.Add(globalStats);

            return new FifaPlayerStatsDto
            {
                StatsPerPlatform = platformsStats.OrderBy(x => x.Platform.Id).ToList(),
                MostWinsTeams = await this.mediator.Send(new GetMostWinsFifaTeamsByPlayerQuery { PlayerId = request.PlayerId, SeasonId = (int)request.SeasonId, NumberOfTeams = 3 }),
                MostLossesTeams = await this.mediator.Send(new GetMostLossesFifaTeamsByPlayerQuery { PlayerId = request.PlayerId, SeasonId = (int)request.SeasonId, NumberOfTeams = 3 }),
                MostPlayedTeams = await this.mediator.Send(new GetMostPlayedFifaTeamsByPlayerQuery { PlayerId = request.PlayerId, SeasonId = (int)request.SeasonId, NumberOfTeams = 3 }),
            };
        }
    }
}
