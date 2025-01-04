// <copyright file="UpdateFifaGamePlayedCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaGamePlayed.Commands.UpdateFifaGamePlayed
{
    using GameOn.Application.Common.Platforms.Queries.GetPlatformById;
    using GameOn.Application.FIFA.FifaTeams.Queries.GetFifaTeamById;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateFifaGamePlayedCommandHandler class.
    /// </summary>
    public class UpdateFifaGamePlayedCommandHandler : IRequestHandler<UpdateFifaGamePlayedCommand, FifaGamePlayed?>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateFifaGamePlayedCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public UpdateFifaGamePlayedCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayed?> Handle(UpdateFifaGamePlayedCommand request, CancellationToken cancellationToken)
        {
            // First, getting game from database
            var gameInDb = await this.context.FifaGamesPlayed.FirstOrDefaultAsync(x => x.Id == request.Game.Id, cancellationToken);

            if (gameInDb is null)
            {
                return null;
            }

            if (gameInDb.IsPlayed != request.Game.IsPlayed)
            {
                gameInDb.PlayedOn = DateTime.UtcNow;
            }

            // Updating game data
            gameInDb.TeamScore1 = request.Game.TeamScore1;
            gameInDb.TeamScore2 = request.Game.TeamScore2;
            gameInDb.IsPlayed = request.Game.IsPlayed;
            gameInDb.Phase = request.Game.Phase;
            gameInDb.TournamentId = request.Game.TournamentId;

            if (gameInDb.TeamScore1 < 0 || gameInDb.TeamScore2 < 0)
            {
                return null;
            }

            // Getting wanted platform
            var platformInDb = await this.mediator.Send(new GetPlatformByIdQuery { PlatformId = request.Game.PlatformId });
            if (platformInDb is null)
            {
                return null;
            }

            gameInDb.PlatformId = platformInDb.Id;

            if (request.Game.FifaTeam1 is not null)
            {
                // Getting Fifa Team 1
                var fifaTeam1InDb = await this.mediator.Send(new GetFifaTeamByIdQuery { Id = (int)request.Game.FifaTeam1 });

                if (fifaTeam1InDb is null)
                {
                    return null;
                }

                gameInDb.Team1Id = fifaTeam1InDb.Id;
            }

            if (request.Game.FifaTeam2 is not null)
            {
                // Getting Fifa Team 1
                var fifaTeam2InDb = await this.mediator.Send(new GetFifaTeamByIdQuery { Id = (int)request.Game.FifaTeam2 });

                if (fifaTeam2InDb is null)
                {
                    return null;
                }

                gameInDb.Team2Id = fifaTeam2InDb.Id;
            }

            // Updating game in database
            this.context.FifaGamesPlayed.Update(gameInDb);
            await this.context.SaveChangesAsync(cancellationToken);

            return gameInDb;
        }
    }
}
