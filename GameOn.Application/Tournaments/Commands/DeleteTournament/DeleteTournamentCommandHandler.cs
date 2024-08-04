// <copyright file="DeleteTournamentCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Tournaments.Commands.DeleteTournament
{
    using GameOn.Application.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// DeleteTournamentCommandHandler class.
    /// </summary>
    public class DeleteTournamentCommandHandler : IRequestHandler<DeleteTournamentCommand, bool>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTournamentCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public DeleteTournamentCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(DeleteTournamentCommand request, CancellationToken cancellationToken)
        {
            var tournamentInDb = await this.context.Tournaments.FirstOrDefaultAsync(x => x.Id == request.TournamentId, cancellationToken);

            if (tournamentInDb == null)
            {
                return false;
            }

            // Updating games (not deleting tournament games)
            var games = await this.context.FifaGamesPlayed.Where(x => x.TournamentId == request.TournamentId).ToListAsync(cancellationToken);

            foreach (var game in games)
            {
                game.TournamentId = null;
            }

            // Deleting tournament player
            var tournamentPlayers = await this.context.TournamentPlayers.Where(x => x.TournamentId == request.TournamentId).ToListAsync(cancellationToken);

            this.context.FifaGamesPlayed.UpdateRange(games);
            this.context.TournamentPlayers.RemoveRange(tournamentPlayers);
            this.context.Tournaments.Remove(tournamentInDb);

            await this.context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
