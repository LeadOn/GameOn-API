// <copyright file="ProcessValorantBackgroundQueueJob.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Services.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Background.Commands.BackfillValorantPlayerHistory;
    using GameOn.Application.Valorant.Background.Commands.RefreshValorantPlayerExtras;
    using GameOn.Application.Valorant.Background.Commands.ReimportValorantMatches;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Single consumer of <see cref="IValorantBackgroundQueue"/>: runs the backfills, reimports and daily
    /// extras one at a time. Every HenrikDev call they make is low priority, so the regular refresh and the
    /// players' clicks always keep a share of the quota.
    /// </summary>
#pragma warning disable SA1009 // Closing parenthesis should not be followed by a space.
    public class ProcessValorantBackgroundQueueJob(IValorantBackgroundQueue queue, IServiceScopeFactory serviceScopeFactory, ILogger<ProcessValorantBackgroundQueueJob> logger) : BackgroundService
#pragma warning restore SA1009 // Closing parenthesis should not be followed by a space.
    {
        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("ProcessValorantBackgroundQueueJob started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await queue.DequeueAsync(stoppingToken);
                logger.LogInformation("Valorant background task {Kind} started (player {PlayerId}).", task.Kind, task.PlayerId);

                // An exception escaping ExecuteAsync stops the whole host: a task that fails is recorded as
                // failed and the line moves on.
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    IRequest<string> command = task.Kind switch
                    {
                        ValBackgroundTaskKind.Backfill => new BackfillValorantPlayerHistoryCommand { PlayerId = task.PlayerId ?? 0 },
                        ValBackgroundTaskKind.Reimport => new ReimportValorantMatchesCommand { PlayerId = task.PlayerId, Since = task.Since },
                        _ => new RefreshValorantPlayerExtrasCommand { PlayerId = task.PlayerId ?? 0 },
                    };

                    var summary = await mediator.Send(command, stoppingToken);
                    queue.Complete(summary, failed: false);
                    logger.LogInformation("Valorant background task {Kind} done: {Summary}", task.Kind, summary);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    queue.Complete(ex.Message, failed: true);
                    logger.LogError(ex, "Valorant background task {Kind} failed (player {PlayerId}).", task.Kind, task.PlayerId);
                }
            }
        }
    }
}
