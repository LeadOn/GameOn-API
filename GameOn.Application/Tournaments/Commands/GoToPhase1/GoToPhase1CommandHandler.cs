// <copyright file="GoToPhase1CommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Tournaments.Commands.GoToPhase1
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Application.TournamentPlayers.Queries.GetTournamentPlayers;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GoToPhase1CommandHandler class.
    /// </summary>
    public class GoToPhase1CommandHandler : IRequestHandler<GoToPhase1Command, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoToPhase1CommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GoToPhase1CommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(GoToPhase1Command request, CancellationToken cancellationToken)
        {
            // Getting tournament
            var tournamentInDb = await this.context.Tournaments.FirstOrDefaultAsync(x => x.Id == request.TournamentId, cancellationToken);

            if (tournamentInDb is null)
            {
                throw new NotImplementedException();
            }

            // Getting players that are registered
            var tournamentPlayers = await this.mediator.Send(new GetTournamentPlayersQuery { TournamentId = request.TournamentId });

            foreach (var player in tournamentPlayers)
            {
                // Looping for every opponent
                foreach (var player2 in tournamentPlayers)
                {
                    if (player.Player.Id != player2.Player.Id)
                    {
                        var newGame = new FifaGamePlayed
                        {
                            CreatedById = player.Player.Id,
                            IsPlayed = false,
                            PlatformId = int.Parse(Environment.GetEnvironmentVariable("DEFAULT_PLATFORM") ?? "1"),
                            PlayedOn = DateTime.UtcNow,
                            SeasonId = int.Parse(Environment.GetEnvironmentVariable("CURRENT_SEASON") ?? "1"),
                            Team1Id = player.Team.Id,
                            Team2Id = player2.Team.Id,
                            Phase = 1,
                            TournamentId = request.TournamentId,
                            TeamPlayers = new List<FifaTeamPlayer>
                                {
                                    new FifaTeamPlayer
                                    {
                                        PlayerId = player.Player.Id,
                                        Team = 0,
                                    },
                                    new FifaTeamPlayer
                                    {
                                        PlayerId = player2.Player.Id,
                                        Team = 1,
                                    },
                                },
                        };

                        this.context.FifaGamesPlayed.Add(newGame);
                        await this.context.SaveChangesAsync(cancellationToken);
                    }
                }
            }

            // Updating tournament to Phase 1
            tournamentInDb.State = TournamentStates.Phase1;
            this.context.Tournaments.Update(tournamentInDb);
            await this.context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
