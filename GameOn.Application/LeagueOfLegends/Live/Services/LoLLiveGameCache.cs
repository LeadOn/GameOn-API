// <copyright file="LoLLiveGameCache.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Live.Services
{
    using System.Net;
    using GameOn.External.Common.Exceptions;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// In-memory implementation of <see cref="ILoLLiveGameCache"/>. Registered as a singleton: the answers
    /// are the state, and they only save calls if every request shares them.
    /// </summary>
    /// <remarks>
    /// There is no rate limiter in front of the Riot clients, so this is where the budget is kept: one call
    /// per account per <see cref="CacheDuration"/> at most, whatever the traffic, and none at all while
    /// nobody is looking. A single lock serializes the refreshes, so that ten simultaneous page loads cost
    /// one round of calls rather than ten.
    /// </remarks>
    public class LoLLiveGameCache : ILoLLiveGameCache
    {
        // The top of the 30 to 60 seconds asked for: every account costs one call per refresh, and a game
        // lasts half an hour, so a minute of lag is invisible where half a minute would double the calls.
        private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

        // How long Riot is left alone after a 429. ExternalApiException doesn't carry the Retry-After header,
        // and a minute covers Riot's shortest windows (1 second and 2 minutes for a development key, 10
        // seconds and 10 minutes for a production one) well enough without starving the page for long.
        private static readonly TimeSpan RateLimitBackoff = TimeSpan.FromSeconds(60);

        private readonly SemaphoreSlim refreshLock = new SemaphoreSlim(1, 1);
        private readonly Dictionary<string, LoLLiveGameSnapshot> snapshots = new Dictionary<string, LoLLiveGameSnapshot>(StringComparer.Ordinal);
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<LoLLiveGameCache> logger;
        private DateTime rateLimitedUntil = DateTime.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoLLiveGameCache"/> class.
        /// </summary>
        /// <param name="serviceScopeFactory">Scope factory, injected: the Riot client is scoped, this cache is not.</param>
        /// <param name="logger">Logger, injected.</param>
        public LoLLiveGameCache(IServiceScopeFactory serviceScopeFactory, ILogger<LoLLiveGameCache> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<string, LoLLiveGameSnapshot>> GetActiveGames(IReadOnlyCollection<string> puuids, CancellationToken cancellationToken)
        {
            await this.refreshLock.WaitAsync(cancellationToken);

            try
            {
                var now = DateTime.UtcNow;
                var expired = puuids
                    .Where(puuid => !this.snapshots.TryGetValue(puuid, out var snapshot) || now - snapshot.RetrievedOn >= CacheDuration)
                    .ToHashSet(StringComparer.Ordinal);

                // While rate limited, the last known answers are served as they are, however old: an account
                // shown in a game that ended a minute ago beats an empty list, and asking again would only
                // push the limit further.
                if (expired.Count > 0 && now >= this.rateLimitedUntil)
                {
                    await this.Refresh(expired, cancellationToken);
                }

                return puuids
                    .Distinct(StringComparer.Ordinal)
                    .Where(this.snapshots.ContainsKey)
                    .ToDictionary(puuid => puuid, puuid => this.snapshots[puuid], StringComparer.Ordinal);
            }
            finally
            {
                this.refreshLock.Release();
            }
        }

        /// <summary>
        /// Asks spectator-v5 about every expired account, one call each, except for the accounts found in a
        /// game already fetched for someone else: that game lists them with their champion and side, so one
        /// call answers for a whole premade.
        /// </summary>
        /// <param name="expired">PUUIDs to refresh. Emptied as they are answered for.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task Refresh(HashSet<string> expired, CancellationToken cancellationToken)
        {
            using var scope = this.serviceScopeFactory.CreateScope();
            var spectatorService = scope.ServiceProvider.GetRequiredService<ISpectatorService>();

            while (expired.Count > 0)
            {
                var puuid = expired.First();
                expired.Remove(puuid);

                CurrentGameInfoDto? game;

                try
                {
                    game = await spectatorService.GetActiveGame(puuid, cancellationToken);
                }
                catch (ExternalApiException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    // Every other call of this round would be refused too. The accounts not asked about yet
                    // keep their previous answer, and the next request after the backoff picks up from there.
                    this.rateLimitedUntil = DateTime.UtcNow + RateLimitBackoff;
                    this.logger.LogWarning("spectator-v5 rate limited: live games not refreshed for {AccountCount} account(s), next attempt after {RetryAfter}.", expired.Count + 1, this.rateLimitedUntil);
                    return;
                }
                catch (Exception ex) when (ex is ExternalApiException or HttpRequestException
                    || (ex is TaskCanceledException && !cancellationToken.IsCancellationRequested))
                {
                    // Riot refused this account alone (a PUUID is tied to the API key that obtained it, and
                    // one obtained with another key answers 400), or didn't answer in time. Shown as not
                    // playing until the next refresh rather than failing the whole list over one account.
                    this.logger.LogWarning(ex, "spectator-v5 could not tell whether {Puuid} is in a game.", puuid);
                    game = null;
                }

                var answer = new LoLLiveGameSnapshot { Game = game, RetrievedOn = DateTime.UtcNow };
                this.snapshots[puuid] = answer;

                if (game is null)
                {
                    continue;
                }

                foreach (var participant in game.Participants)
                {
                    if (participant.Puuid is not null && expired.Remove(participant.Puuid))
                    {
                        this.snapshots[participant.Puuid] = answer;
                    }
                }
            }
        }
    }
}
