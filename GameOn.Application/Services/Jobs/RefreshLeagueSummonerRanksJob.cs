// <copyright file="RefreshLeagueSummonerRanksJob.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Services.Jobs
{
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdateAllPlayerRanks;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Background service that refreshes League of Legends summoner ranks every 30 minutes.
    /// </summary>
    // False positive: StyleCop 1.1.118 predates C# 12 primary constructors and does not recognise
    // the ") : Base" shape on a class declaration. Removing the space to satisfy SA1009 immediately
    // trips SA1024 ("colon should be preceded by a space") on the very next column, verified: the two
    // rules contradict each other here and no source formatting satisfies both.
#pragma warning disable SA1009 // Closing parenthesis should not be followed by a space.
    public class RefreshLeagueSummonerRanksJob(IServiceScopeFactory serviceScopeFactory, ILogger<RefreshLeagueSummonerRanksJob> logger) : BackgroundService
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
            logger.LogInformation("RefreshLeagueSummonerRanksJob started.");

            while (await this.timer.WaitForNextTickAsync(stoppingToken))
            {
                logger.LogInformation("RefreshLeagueSummonerRanksJob tick at {Time} — refreshing summoner ranks.", DateTimeOffset.UtcNow);

                using var scope = serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new UpdateAllPlayerRanksCommand(), stoppingToken);
            }
        }
    }
}
