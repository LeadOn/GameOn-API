// <copyright file="DeclareFifaGamePlayedScoreCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaGamePlayed.Commands.DeclareFifaGamePlayedScore
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// DeclareFifaGamePlayedScoreCommandHandler class.
    /// </summary>
    public class DeclareFifaGamePlayedScoreCommandHandler : IRequestHandler<DeclareFifaGamePlayedScoreCommand, FifaGamePlayed?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeclareFifaGamePlayedScoreCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public DeclareFifaGamePlayedScoreCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayed?> Handle(DeclareFifaGamePlayedScoreCommand request, CancellationToken cancellationToken)
        {
            // First, getting game from database
            var gameInDb = await this.context.FifaGamesPlayed
                .FirstOrDefaultAsync(
                    x =>
                    x.Id == request.ScoreDto.GameId
                    && x.IsPlayed == false
                    && x.TeamPlayers.FirstOrDefault(
                        y => y.PlayerId == request.ScoreDto.CurrentPlayerId) != null,
                    cancellationToken);

            if (gameInDb is null)
            {
                return null;
            }

            // Updating game
            gameInDb.IsPlayed = true;
            gameInDb.TeamScore1 = request.ScoreDto.ScoreTeam1;
            gameInDb.TeamScore2 = request.ScoreDto.ScoreTeam2;
            gameInDb.PlayedOn = DateTime.Now;

            this.context.FifaGamesPlayed.Update(gameInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return gameInDb;
        }
    }
}
