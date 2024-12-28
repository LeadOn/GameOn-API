// <copyright file="UpdateSoccerFiveSurveyCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.SoccerFives.Commands.UpdateSoccerFiveSurvey
{
    using GameOn.Common.Interfaces;
    using GameOn.Application.TournamentPlayers.Queries.GetSoccerFiveById;
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateSoccerFiveSurveyCommandHandler class.
    /// </summary>
    public class UpdateSoccerFiveSurveyCommandHandler : IRequestHandler<UpdateSoccerFiveSurveyCommand, SoccerFiveDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSoccerFiveSurveyCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediatr">Mediator interface, injected.</param>
        public UpdateSoccerFiveSurveyCommandHandler(IApplicationDbContext context, ISender mediatr)
        {
            this.context = context;
            this.mediator = mediatr;
        }

        /// <inheritdoc />
        public async Task<SoccerFiveDto> Handle(UpdateSoccerFiveSurveyCommand request, CancellationToken cancellationToken)
        {
            // First, checking if user is the one who created this five
            var fiveInDb = await this.context.SoccerFives.FirstOrDefaultAsync(x => x.Id == request.Survey.SoccerFiveId && x.CreatedById == request.CurrentPlayerId, cancellationToken);

            if (fiveInDb is null)
            {
                throw new NotImplementedException();
            }

            // Now, let's delete current survey
            var surveyAnswers = await this.context.SoccerFiveVoteAnswers.Where(x => x.VoteChoice.SoccerFiveId == fiveInDb.Id).ToListAsync(cancellationToken);
            this.context.SoccerFiveVoteAnswers.RemoveRange(surveyAnswers);
            var surveyChoices = await this.context.SoccerFiveVoteChoices.Where(x => x.SoccerFiveId == fiveInDb.Id).ToListAsync(cancellationToken);
            this.context.SoccerFiveVoteChoices.RemoveRange(surveyChoices);

            var choicesToAdd = new List<SoccerFiveVoteChoice>();

            // Updating everything
            fiveInDb.VoteQuestion = request.Survey.VoteQuestion;

            foreach (var surveyChoice in request.Survey.VotesChoices)
            {
                var choice = new SoccerFiveVoteChoice
                {
                    SoccerFiveId = fiveInDb.Id,
                    Label = surveyChoice.Label,
                    Order = surveyChoice.Order,
                };
                this.context.SoccerFiveVoteChoices.Add(choice);
            }

            // Saving changes
            await this.context.SaveChangesAsync(cancellationToken);

            // Returning updated DTO
            return await this.mediator.Send(new GetSoccerFiveByIdQuery { SoccerFiveId = fiveInDb.Id }, cancellationToken) ?? throw new NotImplementedException();
        }
    }
}
