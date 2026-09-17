// <copyright file="GetLoLCoachQueueStatusQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Queries.GetLoLCoachQueueStatus
{
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using MediatR;

    /// <summary>
    /// GetLoLCoachQueueStatusQueryHandler class.
    /// </summary>
    public class GetLoLCoachQueueStatusQueryHandler : IRequestHandler<GetLoLCoachQueueStatusQuery, LoLCoachQueueStatusDto?>
    {
        private readonly ILoLCoachQueue queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLoLCoachQueueStatusQueryHandler"/> class.
        /// </summary>
        /// <param name="queue">Coach analysis queue, injected.</param>
        public GetLoLCoachQueueStatusQueryHandler(ILoLCoachQueue queue)
        {
            this.queue = queue;
        }

        /// <inheritdoc />
        public Task<LoLCoachQueueStatusDto?> Handle(GetLoLCoachQueueStatusQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.queue.GetStatus(request.MatchId, request.PlayerId));
        }
    }
}
