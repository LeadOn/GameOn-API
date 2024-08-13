// <copyright file="DeleteSoccerFiveCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>
namespace GameOn.Application.SoccerFives.Commands.DeleteSoccerFive
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// DeleteSoccerFiveCommandHandler class.
    /// </summary>
    public class DeleteSoccerFiveCommandHandler : IRequestHandler<DeleteSoccerFiveCommand, bool>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSoccerFiveCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public DeleteSoccerFiveCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(DeleteSoccerFiveCommand request, CancellationToken cancellationToken)
        {
            var soccerFiveInDb = await this.context.SoccerFives.FirstOrDefaultAsync(x => x.Id == request.SoccerFiveId, cancellationToken);

            if (soccerFiveInDb == null || soccerFiveInDb.CreatedById != request.CurrentPlayerId)
            {
                throw new NotImplementedException();
            }

            var answers = await this.context.SoccerFiveVoteAnswers.Where(x => x.VoteChoice.SoccerFiveId == request.SoccerFiveId).ToListAsync(cancellationToken);
            var choices = await this.context.SoccerFiveVoteChoices.Where(x => x.SoccerFiveId == request.SoccerFiveId).ToListAsync(cancellationToken);

            this.context.SoccerFiveVoteAnswers.RemoveRange(answers);
            this.context.SoccerFiveVoteChoices.RemoveRange(choices);
            this.context.SoccerFives.Remove(soccerFiveInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
