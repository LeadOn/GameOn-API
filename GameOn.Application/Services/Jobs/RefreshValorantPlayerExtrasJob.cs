// <copyright file="RefreshValorantPlayerExtrasJob.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Services.Jobs
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Queues, once a day, the refresh of every Valorant account's accolades, agent mastery and Premier team.
    /// Daily rather than with the 30-minute refresh: those change slowly, cost 3 calls per account, and the
    /// accolades endpoint covers 11 to 20 ranked matches, far more than a day of play.
    /// </summary>
#pragma warning disable SA1009 // Closing parenthesis should not be followed by a space.
    public class RefreshValorantPlayerExtrasJob(IValorantBackgroundQueue queue, IServiceScopeFactory serviceScopeFactory, ILogger<RefreshValorantPlayerExtrasJob> logger) : BackgroundService
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
            logger.LogInformation("RefreshValorantPlayerExtrasJob started.");

            // First pass shortly after start-up, so that a deploy does not wait a day for the first refresh.
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

            do
            {
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var playerIds = await context.Players
                        .Where(x => x.ValorantPuuid != null && !x.Archived)
                        .Select(x => x.Id)
                        .ToListAsync(stoppingToken);

                    foreach (var playerId in playerIds)
                    {
                        queue.Enqueue(new ValBackgroundTaskDto { Kind = ValBackgroundTaskKind.Extras, PlayerId = playerId });
                    }

                    logger.LogInformation("RefreshValorantPlayerExtrasJob queued {Count} accounts.", playerIds.Count);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "RefreshValorantPlayerExtrasJob tick failed, retrying on the next one.");
                }
            }
            while (await this.timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}
