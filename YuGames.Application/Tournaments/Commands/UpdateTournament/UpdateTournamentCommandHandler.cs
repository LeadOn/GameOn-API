// <copyright file="UpdateTournamentCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Commands.UpdateTournament
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Domain;

    /// <summary>
    /// UpdateTournamentCommandHandler class.
    /// </summary>
    public class UpdateTournamentCommandHandler : IRequestHandler<UpdateTournamentCommand, Tournament>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTournamentCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public UpdateTournamentCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Tournament> Handle(UpdateTournamentCommand request, CancellationToken cancellationToken)
        {
            var tournamentInDb = await this.context.Tournaments.FirstOrDefaultAsync(x => x.Id == request.TournamentId, cancellationToken);

            if (tournamentInDb is null)
            {
                throw new NotImplementedException();
            }

            tournamentInDb.Name = request.TournamentDto.Name;
            tournamentInDb.Description = request.TournamentDto.Description;
            tournamentInDb.PlannedFrom = request.TournamentDto.PlannedFrom;
            tournamentInDb.PlannedTo = request.TournamentDto.PlannedTo;
            tournamentInDb.LogoUrl = request.TournamentDto.LogoUrl;
            tournamentInDb.State = request.TournamentDto.State;

            this.context.Tournaments.Update(tournamentInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return tournamentInDb;
        }
    }
}
