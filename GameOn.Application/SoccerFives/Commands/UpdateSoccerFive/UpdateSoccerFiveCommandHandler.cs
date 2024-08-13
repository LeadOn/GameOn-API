// <copyright file="UpdateSoccerFiveCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.SoccerFives.Commands.UpdateSoccerFive
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// CreateTournamentCommandHandler class.
    /// </summary>
    public class UpdateSoccerFiveCommandHandler : IRequestHandler<UpdateSoccerFiveCommand, SoccerFive>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSoccerFiveCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public UpdateSoccerFiveCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<SoccerFive> Handle(UpdateSoccerFiveCommand request, CancellationToken cancellationToken)
        {
            var soccerFiveInDb = await this.context.SoccerFives.FirstOrDefaultAsync(x => x.Id == request.SoccerFiveId, cancellationToken);

            if (soccerFiveInDb is null)
            {
                throw new NotImplementedException();
            }

            soccerFiveInDb.Name = request.Name;
            soccerFiveInDb.Description = request.Description;
            soccerFiveInDb.PlannedOn = request.PlannedOn;

            if (request.State is not null)
            {
                soccerFiveInDb.State = (int)request.State;
            }

            this.context.SoccerFives.Update(soccerFiveInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return soccerFiveInDb;
        }
    }
}
