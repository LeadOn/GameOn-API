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
    // False positive: StyleCop 1.1.118 predates C# 12 primary constructors and does not recognise
    // the ") : Base" shape on a class declaration. Removing the space to satisfy SA1009 immediately
    // trips SA1024 ("colon should be preceded by a space") on the very next column, verified: the two
    // rules contradict each other here and no source formatting satisfies both.
#pragma warning disable SA1009 // Closing parenthesis should not be followed by a space.
    public class RefreshLeagueQueuesJob(IServiceScopeFactory serviceScopeFactory, ILogger<RefreshLeagueQueuesJob> logger) : BackgroundService
#pragma warning restore SA1009 // Closing parenthesis should not be followed by a space.
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

                // An exception escaping ExecuteAsync stops the whole host (BackgroundServiceExceptionBehavior
                // defaults to StopHost), so a third party hiccup on one tick would take the API down with it.
                // The tick is logged and skipped, and the next one tries again.
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(new SyncQueuesCommand(), stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "RefreshLeagueQueuesJob tick failed, retrying on the next one.");
                }
            }
        }
    }
}
