﻿// <copyright file="GetTournamentByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Queries.GetTournamentById
{
    using GameOn.Application.FIFA.TournamentPlayers.Queries.GetTournamentPlayerStats;
    using GameOn.Common.Collections;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetTournamentByIdQueryHandler class.
    /// </summary>
    public class GetTournamentByIdQueryHandler : IRequestHandler<GetTournamentByIdQuery, TournamentDto?>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTournamentByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetTournamentByIdQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<TournamentDto?> Handle(GetTournamentByIdQuery request, CancellationToken cancellationToken)
        {
            var tournamentInDb = await this.context.Tournaments.Include(x => x.Winner).FirstOrDefaultAsync(x => x.Id == request.TournamentId, cancellationToken);

            if (tournamentInDb is null)
            {
                return null;
            }

            var tournament = new TournamentDto(tournamentInDb);

            tournament.Players = await this.context.TournamentPlayers.Include(x => x.FifaTeam).Include(x => x.Player).Where(x => x.TournamentId == request.TournamentId).Select(x => new TournamentPlayerDto
            {
                JoinedAt = x.JoinedAt,
                Player = x.Player,
                Team = x.FifaTeam,
                Score = x.Phase1Score,
            }).ToListAsync(cancellationToken);

            foreach (var player in tournament.Players)
            {
                if (player.Score is null && tournamentInDb.State == TournamentStates.Phase1)
                {
                    player.Score = 0;

                    // Getting their wins
                    var gamesPlayed = await this.context.FifaGamesPlayed
                        .Include(x => x.TeamPlayers)
                        .Where(
                            x => x.TournamentId == request.TournamentId
                                 && x.IsPlayed == true
                                 && x.TeamPlayers.FirstOrDefault(y => y.PlayerId == player.Player.Id) != null)
                        .ToListAsync(cancellationToken);

                    foreach (var game in gamesPlayed)
                    {
                        var team = 0;

                        foreach (var teamPlayer in game.TeamPlayers)
                        {
                            if (teamPlayer.PlayerId == player.Player.Id)
                            {
                                team = teamPlayer.Team;
                            }
                        }

                        if ((team == 0 && game.TeamScore1 > game.TeamScore2) || (team == 1 && game.TeamScore1 < game.TeamScore2))
                        {
                            player.Score += tournamentInDb.WinPoints;
                        }
                        else if (game.TeamScore1 == game.TeamScore2)
                        {
                            player.Score += tournamentInDb.DrawPoints;
                        }
                        else
                        {
                            player.Score += tournamentInDb.LoosePoints;
                        }
                    }
                }
                else if (player.Score == null)
                {
                    player.Score = 0;
                }

                player.Stats = await this.mediator.Send(
                    new GetTournamentPlayerStatsQuery
                    { PlayerId = player.Player.Id, TournamentId = request.TournamentId }, cancellationToken);
            }

            tournament.Players = tournament.Players.OrderByDescending(x => x.Score).ThenByDescending(x => x.Stats.GoalDifference).ToList();

            return tournament;
        }
    }
}
