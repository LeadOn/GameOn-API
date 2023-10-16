// <copyright file="SavePhase1ScoreCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Commands.SavePhase1Score
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Application.TournamentPlayers.Queries.GetTournamentPlayers;
    using YuGames.Domain;

    /// <summary>
    /// SavePhase1ScoreCommandHandler class.
    /// </summary>
    public class SavePhase1ScoreCommandHandler : IRequestHandler<SavePhase1ScoreCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePhase1ScoreCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public SavePhase1ScoreCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(SavePhase1ScoreCommand request, CancellationToken cancellationToken)
        {
            var tournamentInDb =
                await this.context.Tournaments.FirstOrDefaultAsync(x => x.Id == request.TournamentId,
                    cancellationToken);

            if (tournamentInDb is null)
            {
                return false;
            }

            var tournamentPlayers = await this.context.TournamentPlayers
                .Include(x => x.FifaTeam)
                .Include(x => x.Player)
                .Where(x => x.TournamentId == request.TournamentId)
                .ToListAsync(cancellationToken);

            foreach (var player in tournamentPlayers)
            {
                player.Phase1Score = 0;

                // Getting their wins
                var gamesPlayed = await this.context.FifaGamesPlayed
                    .Include(x => x.TeamPlayers)
                    .Where(
                        x => x.TournamentId == request.TournamentId
                             && x.Phase == 1
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

                    if ((team == 0 && game.TeamScore1 > game.TeamScore2) ||
                        (team == 1 && game.TeamScore1 < game.TeamScore2))
                    {
                        player.Phase1Score += 3;
                    }
                    else if (game.TeamScore1 == game.TeamScore2)
                    {
                        player.Phase1Score += 1;
                    }
                }
            }

            this.context.TournamentPlayers.UpdateRange(tournamentPlayers);
            await this.context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
