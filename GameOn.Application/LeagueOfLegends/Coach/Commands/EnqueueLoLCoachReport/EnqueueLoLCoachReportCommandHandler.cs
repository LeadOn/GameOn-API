// <copyright file="EnqueueLoLCoachReportCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Commands.EnqueueLoLCoachReport
{
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// EnqueueLoLCoachReportCommandHandler class.
    /// </summary>
    /// <remarks>
    /// Returns as soon as the ticket is in the line: the model is never called from a request any more. The
    /// existence check happens here rather than in the consumer so that asking for an analysis of a game
    /// somebody did not play is refused while there is still a caller to tell, instead of failing silently in
    /// a background loop minutes later.
    /// </remarks>
    public class EnqueueLoLCoachReportCommandHandler : IRequestHandler<EnqueueLoLCoachReportCommand, LoLCoachQueueStatusDto?>
    {
        private readonly IApplicationDbContext context;
        private readonly ILoLCoachQueue queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnqueueLoLCoachReportCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="queue">Coach analysis queue, injected.</param>
        public EnqueueLoLCoachReportCommandHandler(IApplicationDbContext context, ILoLCoachQueue queue)
        {
            this.context = context;
            this.queue = queue;
        }

        /// <inheritdoc />
        public async Task<LoLCoachQueueStatusDto?> Handle(EnqueueLoLCoachReportCommand request, CancellationToken cancellationToken)
        {
            var played = await this.context.LeagueOfLegendsGameParticipants
                .AnyAsync(x => x.MatchId == request.MatchId && x.PlayerId == request.PlayerId, cancellationToken);

            if (!played)
            {
                // Either the match is unknown, or that player simply did not play it.
                return null;
            }

            return this.queue.Enqueue(request.MatchId, request.PlayerId, request.ForceRegenerate);
        }
    }
}
