// <copyright file="RefreshLeagueQueuesJob.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Services.Jobs
{
    using GameOn.Application.LeagueOfLegends.Queues.Commands.SyncQueues;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Background service that refreshes League of Legends queue types once a day.
    /// </summary>
    public class RefreshLeagueQueuesJob(IServiceScopeFactory serviceScopeFactory, ILogger<RefreshLeagueQueuesJob> logger) : BackgroundService
    {
        private readonly PeriodicTimer timer = new (TimeSpan.FromHours(24));

        /// <inheritdoc/>
        public override void Dispose()
        {
            this.timer.Dispose();
            base.Dispose();
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("RefreshLeagueQueuesJob started.");

            while (await this.timer.WaitForNextTickAsync(stoppingToken))
            {
                logger.LogInformation("RefreshLeagueQueuesJob tick at {Time} — refreshing queue types.", DateTimeOffset.UtcNow);

                using var scope = serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new SyncQueuesCommand(), stoppingToken);
            }
        }
    }
}
