// <copyright file="GetTournamentPlayerStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.TournamentPlayers.Queries.GetTournamentPlayerStats
{
    using GameOn.Application.FifaGamePlayed.Queries.GetFifaGamePlayedById;
    using GameOn.Application.Players.Queries.GetPlayerById;
    using GameOn.Application.TeamPlayers.Queries.SearchTeamPlayer;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;

    /// <summary>
    /// GetTournamentPlayerStatsQueryHandler class.
    /// </summary>
    public class GetTournamentPlayerStatsQueryHandler : IRequestHandler<GetTournamentPlayerStatsQuery, PlatformStatsDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTournamentPlayerStatsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetTournamentPlayerStatsQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<PlatformStatsDto> Handle(GetTournamentPlayerStatsQuery request, CancellationToken cancellationToken)
        {
            // Get player from database.
            var playerInDb = await mediator.Send(new GetPlayerByIdQuery { PlayerId = request.PlayerId }, cancellationToken);

            if (playerInDb is null)
            {
                throw new NotImplementedException();
            }

            var stats = new PlatformStatsDto { AverageGoalGiven = 0, AverageGoalTaken = 0, Draws = 0, GoalDifference = 0, Losses = 0, Wins = 0, GoalsGiven = 0, GoalsTaken = 0 };

            // Getting games played by platform
            var teamPlayersInDb = await mediator.Send(
                new SearchTeamPlayerQuery
                {
                    Query = x => x.FifaGamePlayed.TournamentId == request.TournamentId
                                 && x.PlayerId == playerInDb.Id
                                 && x.FifaGamePlayed.IsPlayed == true,
                    Limit = 1000000,
                }, cancellationToken);

            var gamesNotPlayed = await mediator.Send(
                new SearchTeamPlayerQuery
                {
                    Query = x => x.FifaGamePlayed.TournamentId == request.TournamentId
                                 && x.PlayerId == playerInDb.Id
                                 && x.FifaGamePlayed.IsPlayed == false,
                    Limit = 1000000,
                }, cancellationToken);

            stats.MatchPlayed = teamPlayersInDb.Count();
            stats.MatchNotPlayed = gamesNotPlayed.Count();
            stats.TotalMatch = stats.MatchPlayed + stats.MatchNotPlayed;
            if (stats.TotalMatch == 0)
            {
                stats.Progression = 0;
            }
            else
            {
                stats.Progression = (float)Math.Round((double)(stats.MatchPlayed * 100 / (float)stats.TotalMatch), 2);
            }

            if (teamPlayersInDb.ToList().Count > 0)
            {
                // For each game played, getting that stats
                foreach (var teamPlayer in teamPlayersInDb)
                {
                    // Getting game
                    var gameInDb = await mediator.Send(new GetFifaGamePlayedByIdQuery { FifaGamePlayedId = teamPlayer.FifaGameId }, cancellationToken);

                    if (gameInDb is null)
                    {
                        throw new NotImplementedException();
                    }

                    if (gameInDb.Team1.Score == gameInDb.Team2.Score)
                    {
                        stats.Draws++;
                    }
                    else if (teamPlayer.Team == 0 && gameInDb.Team1.Score > gameInDb.Team2.Score || teamPlayer.Team == 1 && gameInDb.Team1.Score < gameInDb.Team2.Score)
                    {
                        stats.Wins++;
                    }
                    else if (teamPlayer.Team == 0 && gameInDb.Team1.Score < gameInDb.Team2.Score || teamPlayer.Team == 1 && gameInDb.Team1.Score > gameInDb.Team2.Score)
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
                        stats.GoalsTaken += gameInDb.Team1.Score;
                        stats.GoalsGiven += gameInDb.Team2.Score;
                    }
                }

                stats.GoalDifference = stats.GoalsGiven - stats.GoalsTaken;
                if (stats.GoalsGiven == 0 || stats.Wins + stats.Losses + stats.Draws == 0)
                {
                    stats.AverageGoalGiven = 0;
                }
                else
                {
                    stats.AverageGoalGiven = (float)Math.Round((double)(stats.GoalsGiven / (float)(stats.Wins + stats.Draws + stats.Losses)), 2);
                }

                if (stats.GoalsTaken == 0 || stats.Wins + stats.Losses + stats.Draws == 0)
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
                    stats.WinRate = (float)Math.Round((double)(stats.Wins * 100 / (float)gamesPlayed), 2);
                }

                // Calculating loose rate
                if (stats.Losses == 0 || gamesPlayed == 0)
                {
                    stats.LooseRate = 0;
                }
                else
                {
                    stats.LooseRate = (float)Math.Round((double)(stats.Losses * 100 / (float)gamesPlayed), 2);
                }

                // Calculating draw rate
                if (stats.Draws == 0 || gamesPlayed == 0)
                {
                    stats.DrawRate = 0;
                }
                else
                {
                    stats.DrawRate = (float)Math.Round((double)(stats.Draws * 100 / (float)gamesPlayed), 2);
                }
            }

            return stats;
        }
    }
}
