// <copyright file="ProcessLoLCoachQueueJob.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Services.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.LeagueOfLegends.Coach.Commands.GenerateLoLCoachReport;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using GameOn.External.Llm.Exceptions;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Background service that writes the coach analyses players have asked for, one at a time.
    /// </summary>
    /// <remarks>
    /// The single consumer is the rate limit. With one generation running at a time and the provider's own
    /// pacing floor inside the LLM service, calls leave this process slowly enough for a five-per-minute
    /// allowance without anything having to track a sliding window. Nothing is ever generated on a schedule or
    /// by scanning the database: this loop only ever writes what somebody pressed a button for.
    /// </remarks>
    // False positive: StyleCop 1.1.118 predates C# 12 primary constructors and does not recognise
    // the ") : Base" shape on a class declaration. Removing the space to satisfy SA1009 immediately
    // trips SA1024 ("colon should be preceded by a space") on the very next column, verified: the two
    // rules contradict each other here and no source formatting satisfies both.
#pragma warning disable SA1009 // Closing parenthesis should not be followed by a space.
    public class ProcessLoLCoachQueueJob(IServiceScopeFactory serviceScopeFactory, ILoLCoachQueue queue, ILogger<ProcessLoLCoachQueueJob> logger) : BackgroundService
#pragma warning restore SA1009 // Closing parenthesis should not be followed by a space.
    {
        /// <summary>
        /// How many refusals a single analysis may collect before it is dropped. Without a ceiling, one
        /// permanently refused ticket would sit at the head of the line and starve everything behind it.
        /// </summary>
        private const int MaxAttempts = 5;

        /// <summary>
        /// How long to stand down after the provider turns a generation away. Comfortably longer than the
        /// pacing floor, because a refusal means the allowance is already spent and trying sooner would only
        /// spend the next one on another refusal.
        /// </summary>
        private static readonly TimeSpan TransientBackoff = TimeSpan.FromSeconds(30);

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("ProcessLoLCoachQueueJob started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                LoLCoachQueueTicket ticket;

                try
                {
                    ticket = await queue.DequeueAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                await this.WriteReport(ticket, stoppingToken);
            }

            logger.LogInformation("ProcessLoLCoachQueueJob stopped.");
        }

        /// <summary>
        /// Writes one analysis, putting it back in the line when the provider is the one saying no.
        /// </summary>
        /// <param name="ticket">Analysis to write.</param>
        /// <param name="stoppingToken">Token to stop all async execution.</param>
        /// <returns>Nothing.</returns>
        private async Task WriteReport(LoLCoachQueueTicket ticket, CancellationToken stoppingToken)
        {
            var startedOn = DateTime.UtcNow;
            var done = true;

            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(
                    new GenerateLoLCoachReportCommand
                    {
                        MatchId = ticket.MatchId,
                        PlayerId = ticket.PlayerId,
                        ForceRegenerate = ticket.ForceRegenerate,
                    },
                    stoppingToken);

                logger.LogInformation(
                    "Coach analysis written for match {MatchId} and player {PlayerId} in {Elapsed:0}s.",
                    ticket.MatchId,
                    ticket.PlayerId,
                    (DateTime.UtcNow - startedOn).TotalSeconds);
            }
            catch (LlmTransientException ex) when (ticket.Attempts + 1 < MaxAttempts)
            {
                // The provider passed on it: this is a "later", not a failure, and the player keeps their place.
                logger.LogWarning(
                    ex,
                    "Coach analysis for match {MatchId} and player {PlayerId} was turned away, attempt {Attempt} of {MaxAttempts}.",
                    ticket.MatchId,
                    ticket.PlayerId,
                    ticket.Attempts + 1,
                    MaxAttempts);

                queue.Retry(ticket);
                done = false;

                await Task.Delay(TransientBackoff, stoppingToken);
            }
            catch (Exception ex)
            {
                // Everything else - an exhausted retry budget included - drops the ticket. Leaving it in would
                // block every analysis behind it, and the player sees a 404 again, which the front reads as
                // "offer the button".
                logger.LogError(
                    ex,
                    "Coach analysis for match {MatchId} and player {PlayerId} was abandoned.",
                    ticket.MatchId,
                    ticket.PlayerId);
            }
            finally
            {
                if (done)
                {
                    queue.Complete(ticket, DateTime.UtcNow - startedOn);
                }
            }
        }
    }
}
