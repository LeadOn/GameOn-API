// <copyright file="GetValorantBackgroundQueueStatusQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Queries.GetValorantBackgroundQueueStatus
{
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;
    using MediatR;

    /// <summary>
    /// <see cref="GetValorantBackgroundQueueStatusQuery"/> handler.
    /// </summary>
    public class GetValorantBackgroundQueueStatusQueryHandler : IRequestHandler<GetValorantBackgroundQueueStatusQuery, ValBackgroundQueueStatusDto>
    {
        private readonly IValorantBackgroundQueue queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValorantBackgroundQueueStatusQueryHandler"/> class.
        /// </summary>
        /// <param name="queue">Valorant background queue, injected.</param>
        public GetValorantBackgroundQueueStatusQueryHandler(IValorantBackgroundQueue queue)
        {
            this.queue = queue;
        }

        /// <inheritdoc />
        public Task<ValBackgroundQueueStatusDto> Handle(GetValorantBackgroundQueueStatusQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.queue.GetStatus());
        }
    }
}
