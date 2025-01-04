// <copyright file="CreateFifaGamePlayedCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Commands.CreateFifaGamePlayed
{
    using GameOn.Application.Common.Players.Queries.GetPlayerById;
    using GameOn.Application.Common.Players.Queries.GetPlayerByKeycloakId;
    using GameOn.Application.FifaTeams.Queries.GetFifaTeamById;
    using GameOn.Application.Platforms.Queries.GetPlatformById;
    using GameOn.Application.TeamPlayers.Commands.CreateTeamPlayer;
    using GameOn.Common.Exceptions;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// ConvertFifaGamePlayedToDtoCommandHandler class.
    /// </summary>
    public class CreateFifaGamePlayedCommandHandler : IRequestHandler<CreateFifaGamePlayedCommand, FifaGamePlayed?>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFifaGamePlayedCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public CreateFifaGamePlayedCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayed?> Handle(CreateFifaGamePlayedCommand request, CancellationToken cancellationToken)
        {
            // First, getting that platform
            var platformInDb = await this.mediator.Send(new GetPlatformByIdQuery { PlatformId = request.NewGame.PlatformId });

            if (platformInDb is null)
            {
                return null;
            }

            // Then, getting creator profile
            var creatorInDb = await this.mediator.Send(new GetPlayerByKeycloakIdQuery { KeycloakId = request.NewGame.KeycloakId });

            if (creatorInDb is null)
            {
                return null;
            }

            // Now, getting all player accounts
            foreach (var playerId in request.NewGame.Team1)
            {
                var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = int.Parse(playerId) });
                if (playerInDb is null)
                {
                    return null;
                }
            }

            foreach (var playerId in request.NewGame.Team2)
            {
                var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = int.Parse(playerId) });
                if (playerInDb is null)
                {
                    return null;
                }
            }

            var newGame = new FifaGamePlayed
            {
                PlatformId = platformInDb.Id,
                PlayedOn = request.NewGame.CreatedOn,
                TeamScore1 = request.NewGame.TeamScore1,
                TeamScore2 = request.NewGame.TeamScore2,
                CreatedById = creatorInDb.Id,
                IsPlayed = true,
                SeasonId = int.Parse(Environment.GetEnvironmentVariable("CURRENT_SEASON") ?? throw new MissingEnvironmentVariableException("CURRENT_SEASON")),
            };

            if (newGame.TeamScore1 < 0 || newGame.TeamScore2 < 0)
            {
                return null;
            }

            // Checking if fifa teams are in Database.
            if (request.NewGame.FifaTeam1 is not null && request.NewGame.FifaTeam1 != 0)
            {
                var fifaTeam1 = await this.mediator.Send(new GetFifaTeamByIdQuery { Id = (int)request.NewGame.FifaTeam1 });
                if (fifaTeam1 is not null)
                {
                    newGame.Team1Id = fifaTeam1.Id;
                }
            }

            if (request.NewGame.FifaTeam2 is not null && request.NewGame.FifaTeam2 != 0)
            {
                var fifaTeam2 = await this.mediator.Send(new GetFifaTeamByIdQuery { Id = (int)request.NewGame.FifaTeam2 });
                if (fifaTeam2 is not null)
                {
                    newGame.Team2Id = fifaTeam2.Id;
                }
            }

            // Now that every player as been found, creating elements in db
            this.context.FifaGamesPlayed.Add(newGame);
            await this.context.SaveChangesAsync(cancellationToken);

            // Now creating team players
            foreach (var playerId in request.NewGame.Team1)
            {
                var teamPlayer = new FifaTeamPlayer
                {
                    FifaGameId = newGame.Id,
                    PlayerId = int.Parse(playerId),
                    Team = 0,
                };

                var teamPlayerInDb = await this.mediator.Send(new CreateTeamPlayerCommand { TeamPlayer = teamPlayer });

                if (teamPlayerInDb is null)
                {
                    return null;
                }
            }

            foreach (var playerId in request.NewGame.Team2)
            {
                var teamPlayer = new FifaTeamPlayer
                {
                    FifaGameId = newGame.Id,
                    PlayerId = int.Parse(playerId),
                    Team = 1,
                };

                var teamPlayerInDb = await this.mediator.Send(new CreateTeamPlayerCommand { TeamPlayer = teamPlayer });

                if (teamPlayerInDb is null)
                {
                    return null;
                }
            }

            return newGame;
        }
    }
}
