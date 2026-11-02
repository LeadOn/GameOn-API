// <copyright file="EnqueueValorantBackgroundTasksCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.EnqueueValorantBackgroundTasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="EnqueueValorantBackgroundTasksCommand"/> handler.
    /// </summary>
    public class EnqueueValorantBackgroundTasksCommandHandler : IRequestHandler<EnqueueValorantBackgroundTasksCommand, ValBackgroundQueueStatusDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IValorantBackgroundQueue queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnqueueValorantBackgroundTasksCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="queue">Valorant background queue, injected.</param>
        public EnqueueValorantBackgroundTasksCommandHandler(IApplicationDbContext context, IValorantBackgroundQueue queue)
        {
            this.context = context;
            this.queue = queue;
        }

        /// <inheritdoc />
        public async Task<ValBackgroundQueueStatusDto> Handle(EnqueueValorantBackgroundTasksCommand request, CancellationToken cancellationToken)
        {
            List<int?> playerIds;

            if (request.PlayerId is not null || request.Kind == ValBackgroundTaskKind.Reimport)
            {
                playerIds = new List<int?> { request.PlayerId };
            }
            else
            {
                playerIds = await this.context.Players
                    .Where(x => x.ValorantPuuid != null && !x.Archived)
                    .OrderBy(x => x.Id)
                    .Select(x => (int?)x.Id)
                    .ToListAsync(cancellationToken);
            }

            foreach (var playerId in playerIds)
            {
                this.queue.Enqueue(new ValBackgroundTaskDto { Kind = request.Kind, PlayerId = playerId, Since = request.Since });
            }

            return this.queue.GetStatus();
        }
    }
}
