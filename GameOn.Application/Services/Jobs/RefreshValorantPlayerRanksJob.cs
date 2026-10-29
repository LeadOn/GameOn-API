// <copyright file="RefreshValorantPlayerRanksJob.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Services.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Players.Commands.UpdateAllValorantPlayerRanks;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Background service that refreshes Valorant player profiles and ranks every 30 minutes.
    /// </summary>
#pragma warning disable SA1009 // Closing parenthesis should not be followed by a space.
    public class RefreshValorantPlayerRanksJob(IServiceScopeFactory serviceScopeFactory, ILogger<RefreshValorantPlayerRanksJob> logger) : BackgroundService
#pragma warning restore SA1009 // Closing parenthesis should not be followed by a space.
    {
        private readonly PeriodicTimer timer = new (TimeSpan.FromMinutes(30));

        /// <inheritdoc/>
        public override void Dispose()
        {
            this.timer.Dispose();
            base.Dispose();
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("RefreshValorantPlayerRanksJob started.");

            while (await this.timer.WaitForNextTickAsync(stoppingToken))
            {
                logger.LogInformation("RefreshValorantPlayerRanksJob tick at {Time} — refreshing Valorant ranks.", DateTimeOffset.UtcNow);

                // An exception escaping ExecuteAsync stops the whole host (BackgroundServiceExceptionBehavior
                // defaults to StopHost), so a third party hiccup on one tick would take the API down with it.
                // The tick is logged and skipped, and the next one tries again.
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(new UpdateAllValorantPlayerRanksCommand(), stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "RefreshValorantPlayerRanksJob tick failed, retrying on the next one.");
                }
            }
        }
    }
}
