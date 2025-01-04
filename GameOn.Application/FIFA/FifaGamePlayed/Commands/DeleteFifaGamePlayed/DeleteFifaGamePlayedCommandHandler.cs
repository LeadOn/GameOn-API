// <copyright file="DeleteFifaGamePlayedCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaGamePlayed.Commands.DeleteFifaGamePlayed
{
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// DeleteFifaGamePlayedCommandHandler class.
    /// </summary>
    public class DeleteFifaGamePlayedCommandHandler : IRequestHandler<DeleteFifaGamePlayedCommand, bool>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFifaGamePlayedCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public DeleteFifaGamePlayedCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(DeleteFifaGamePlayedCommand request, CancellationToken cancellationToken)
        {
            var gameInDb = await this.context.FifaGamesPlayed.FirstOrDefaultAsync(x => x.Id == request.GameId);

            if (gameInDb is null)
            {
                return false;
            }

            var highlights = await this.context.Highlights.Where(x => x.FifaGameId == request.GameId).ToListAsync(cancellationToken);
            var teamPlayers = await this.context.FifaTeamPlayers.Where(x => x.FifaGameId == request.GameId).ToListAsync(cancellationToken);

            this.context.Highlights.RemoveRange(highlights);
            this.context.FifaTeamPlayers.RemoveRange(teamPlayers);
            this.context.FifaGamesPlayed.Remove(gameInDb);
            await this.context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
