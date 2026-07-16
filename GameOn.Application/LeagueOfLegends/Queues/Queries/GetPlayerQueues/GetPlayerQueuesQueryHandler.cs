// <copyright file="GetPlayerQueuesQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Queues.Queries.GetPlayerQueues
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetPlayerQueuesQueryHandler class.
    /// </summary>
    public class GetPlayerQueuesQueryHandler : IRequestHandler<GetPlayerQueuesQuery, IEnumerable<LoLQueue>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPlayerQueuesQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetPlayerQueuesQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LoLQueue>> Handle(GetPlayerQueuesQuery request, CancellationToken cancellationToken)
        {
            var playedQueueIds = await this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.PlayerId == request.PlayerId && x.Game.QueueId != null)
                .Select(x => x.Game.QueueId!.Value)
                .Distinct()
                .ToListAsync(cancellationToken);

            return await this.context.LeagueOfLegendsQueues
                .Where(x => playedQueueIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
