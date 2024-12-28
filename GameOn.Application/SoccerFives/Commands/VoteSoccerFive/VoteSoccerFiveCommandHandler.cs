// <copyright file="VoteSoccerFiveCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.SoccerFives.Commands.VoteSoccerFive
{
    using GameOn.Common.Interfaces;
    using GameOn.Application.TournamentPlayers.Queries.GetSoccerFiveById;
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// VoteSoccerFiveCommandHandler class.
    /// </summary>
    public class VoteSoccerFiveCommandHandler : IRequestHandler<VoteSoccerFiveCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoteSoccerFiveCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediatr">Mediator interface, injected.</param>
        public VoteSoccerFiveCommandHandler(IApplicationDbContext context, ISender mediatr)
        {
            this.context = context;
            this.mediator = mediatr;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(VoteSoccerFiveCommand request, CancellationToken cancellationToken)
        {
            // First, cleaning old votes
            var oldVotes = await this.context.SoccerFiveVoteAnswers.Where(x => x.PlayerId == request.Vote.PlayerId && x.VoteChoice.SoccerFiveId == request.Vote.SoccerFiveId).ToListAsync(cancellationToken);

            var newVotesToSkip = new List<int>();

            oldVotes.ForEach(oldVote =>
            {
                if (!request.Vote.ChoiceIds.Contains(oldVote.VoteChoiceId))
                {
                    this.context.SoccerFiveVoteAnswers.Remove(oldVote);
                }
                else
                {
                    newVotesToSkip.Add(oldVote.VoteChoiceId);
                }
            });

            // Then, adding new votes
            request.Vote.ChoiceIds.ForEach(choiceId =>
            {
                if (!newVotesToSkip.Contains(choiceId))
                {
                    this.context.SoccerFiveVoteAnswers.Add(new SoccerFiveVoteAnswer
                    {
                        PlayerId = request.Vote.PlayerId ?? throw new NotImplementedException(),
                        VoteChoiceId = choiceId,
                    });
                }
            });

            await this.context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
