// <copyright file="SyncQueuesCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Queues.Commands.SyncQueues
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// SyncQueuesCommandHandler class.
    /// </summary>
    public class SyncQueuesCommandHandler : IRequestHandler<SyncQueuesCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly IQueueService queueService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncQueuesCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="queueService">IQueueService interface, injected.</param>
        public SyncQueuesCommandHandler(IApplicationDbContext context, IQueueService queueService)
        {
            this.context = context;
            this.queueService = queueService;
        }

        /// <inheritdoc />
        public async Task Handle(SyncQueuesCommand request, CancellationToken cancellationToken)
        {
            var queues = await this.queueService.GetQueues(cancellationToken);

            foreach (var queue in queues)
            {
                var queueInDb = await this.context.LeagueOfLegendsQueues.FirstOrDefaultAsync(x => x.Id == queue.QueueId, cancellationToken);

                if (queueInDb is null)
                {
                    this.context.LeagueOfLegendsQueues.Add(new LoLQueue
                    {
                        Id = queue.QueueId,
                        Map = queue.Map,
                        Description = queue.Description,
                        Notes = queue.Notes,
                    });
                }
                else
                {
                    queueInDb.Map = queue.Map;
                    queueInDb.Description = queue.Description;
                    queueInDb.Notes = queue.Notes;

                    this.context.LeagueOfLegendsQueues.Update(queueInDb);
                }
            }

            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
