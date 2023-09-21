// <copyright file="GoToPhase1CommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Commands.GoToPhase1
{
    using MediatR;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Application.FifaGamePlayed.Commands.CreateFifaGamePlayed;
    using YuGames.Application.Tournaments.Queries.GetTournamentById;
    using YuGames.Application.Tournaments.Queries.GetTournamentPlayers;
    using YuGames.Domain;

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
            var tournamentInDb = await this.mediator.Send(new GetTournamentByIdQuery { TournamentId = request.TournamentId });

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
                            TeamCode1 = "???",
                            TeamCode2 = "???",
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

                        await this.mediator.Send(new CreateFifaGamePlayedCommand { NewGame = newGame });
                    }
                }
            }

            // Updating tournament to Phase 1
            tournamentInDb.State = TournamentStates.Phase1;
            await this.tournamentRepository.UpdateTournament(tournamentInDb);

            return true;
        }
    }
}
