// <copyright file="DeleteSoccerFiveCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>
namespace GameOn.Application.FIFA.SoccerFives.Commands.DeleteSoccerFive
{
    using GameOn.Common.Interfaces;
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
            var soccerFiveInDb = await context.SoccerFives.FirstOrDefaultAsync(x => x.Id == request.SoccerFiveId, cancellationToken);

            if (soccerFiveInDb == null || soccerFiveInDb.CreatedById != request.CurrentPlayerId)
            {
                throw new NotImplementedException();
            }

            var answers = await context.SoccerFiveVoteAnswers.Where(x => x.VoteChoice.SoccerFiveId == request.SoccerFiveId).ToListAsync(cancellationToken);
            var choices = await context.SoccerFiveVoteChoices.Where(x => x.SoccerFiveId == request.SoccerFiveId).ToListAsync(cancellationToken);

            context.SoccerFiveVoteAnswers.RemoveRange(answers);
            context.SoccerFiveVoteChoices.RemoveRange(choices);
            context.SoccerFives.Remove(soccerFiveInDb);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
