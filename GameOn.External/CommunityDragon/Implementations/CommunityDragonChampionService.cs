// <copyright file="CommunityDragonChampionService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.CommunityDragon.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.Common;
    using GameOn.External.CommunityDragon.Interfaces;
    using GameOn.External.CommunityDragon.Models.DTOs;

    /// <summary>
    /// CommunityDragonChampionService class.
    /// </summary>
    public class CommunityDragonChampionService : HttpServiceBase, ICommunityDragonChampionService
    {
        // The champion list only moves on patch day, and every participant of every imported game
        // needs it: cached process-wide rather than fetched again on each import.
        private static readonly SemaphoreSlim CacheLock = new SemaphoreSlim(1, 1);
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

        private static IReadOnlyDictionary<int, string>? cachedChampions;
        private static DateTime cachedOn = DateTime.MinValue;

        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityDragonChampionService"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public CommunityDragonChampionService(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<int, string>> GetChampionNamesById(CancellationToken cancellationToken)
        {
            if (cachedChampions is not null && DateTime.UtcNow - cachedOn < CacheDuration)
            {
                return cachedChampions;
            }

            await CacheLock.WaitAsync(cancellationToken);

            try
            {
                if (cachedChampions is not null && DateTime.UtcNow - cachedOn < CacheDuration)
                {
                    return cachedChampions;
                }

                var request = new HttpRequestMessage(HttpMethod.Get, "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-summary.json");
                var champions = await RunRequest<IEnumerable<CommunityDragonChampionDto>>(this.client, request, cancellationToken);

                // ID -1 is the "None" placeholder Community Dragon puts at the top of the list.
                cachedChampions = (champions ?? Enumerable.Empty<CommunityDragonChampionDto>())
                    .Where(x => x.Id > 0 && !string.IsNullOrWhiteSpace(x.Alias))
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.First().Alias!);

                cachedOn = DateTime.UtcNow;

                return cachedChampions;
            }
            finally
            {
                CacheLock.Release();
            }
        }
    }
}
