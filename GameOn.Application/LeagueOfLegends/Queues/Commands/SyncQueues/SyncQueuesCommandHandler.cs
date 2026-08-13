// <copyright file="SyncQueuesCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Queues.Commands.SyncQueues
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.CommunityDragon.Interfaces;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// SyncQueuesCommandHandler class.
    /// </summary>
    public class SyncQueuesCommandHandler : IRequestHandler<SyncQueuesCommand>
    {
        /// <summary>
        /// Notes value stamped on queues that only exist in Community Dragon's dataset, so that
        /// entries backfilled this way stay distinguishable from Riot's own <see cref="QueueDto.Notes"/>.
        /// </summary>
        private const string CommunityDragonOnlyNotes = "Complété depuis Community Dragon (absent du référentiel officiel Riot Games).";

        private readonly IApplicationDbContext context;
        private readonly IQueueService queueService;
        private readonly ICommunityDragonQueueService communityDragonQueueService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncQueuesCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="queueService">IQueueService interface, injected.</param>
        /// <param name="communityDragonQueueService">ICommunityDragonQueueService interface, injected.</param>
        public SyncQueuesCommandHandler(IApplicationDbContext context, IQueueService queueService, ICommunityDragonQueueService communityDragonQueueService)
        {
            this.context = context;
            this.queueService = queueService;
            this.communityDragonQueueService = communityDragonQueueService;
        }

        /// <inheritdoc />
        public async Task Handle(SyncQueuesCommand request, CancellationToken cancellationToken)
        {
            // Riot's official queues.json is the source of truth when a queue ID is known there, but it is
            // notoriously stale/incomplete (hundreds of legacy/rotating queue IDs are missing). Community Dragon's
            // mirror is unofficial but far more exhaustive, so it is only used to backfill the IDs Riot doesn't list.
            var riotQueues = (await this.queueService.GetQueues(cancellationToken)).ToList();
            var knownQueueIds = riotQueues.Select(x => x.QueueId).ToHashSet();

            var communityDragonQueues = await this.communityDragonQueueService.GetQueues(cancellationToken);

            var missingQueues = communityDragonQueues
                .Where(x => !knownQueueIds.Contains(x.Id))
                .GroupBy(x => x.Id)
                .Select(group => group
                    .OrderByDescending(x => (x.DetailedDescription ?? x.Description ?? x.Name ?? string.Empty).Length)
                    .First())
                .Select(x => new QueueDto
                {
                    QueueId = x.Id,
                    Map = string.Empty,
                    Description = x.DetailedDescription ?? x.Description ?? x.Name,
                    Notes = CommunityDragonOnlyNotes,
                });

            var queues = riotQueues.Concat(missingQueues);

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
