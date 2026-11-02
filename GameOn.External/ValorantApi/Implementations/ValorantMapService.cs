// <copyright file="ValorantMapService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.ValorantApi.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.Common;
    using GameOn.External.Common.Exceptions;
    using GameOn.External.ValorantApi.Interfaces;
    using GameOn.External.ValorantApi.Models.DTOs;

    /// <summary>
    /// <see cref="IValorantMapService"/> implementation backed by <c>valorant-api.com/v1/maps</c>.
    /// </summary>
    public class ValorantMapService : HttpServiceBase, IValorantMapService
    {
        // Maps only change on patch day, and every kill of every imported match needs them: cached
        // process-wide rather than fetched again on each import.
        private static readonly SemaphoreSlim CacheLock = new SemaphoreSlim(1, 1);
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

        private static IReadOnlyDictionary<string, ValorantApiMapDto>? cachedMaps;
        private static DateTime cachedOn = DateTime.MinValue;

        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantMapService"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public ValorantMapService(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<string, ValorantApiMapDto>> GetMapsById(CancellationToken cancellationToken)
        {
            if (cachedMaps is not null && DateTime.UtcNow - cachedOn < CacheDuration)
            {
                return cachedMaps;
            }

            await CacheLock.WaitAsync(cancellationToken);

            try
            {
                if (cachedMaps is not null && DateTime.UtcNow - cachedOn < CacheDuration)
                {
                    return cachedMaps;
                }

                var request = new HttpRequestMessage(HttpMethod.Get, "https://valorant-api.com/v1/maps");
                var response = await RunRequest<ValorantApiResponseDto<List<ValorantApiMapDto>>>(this.client, request, cancellationToken);

                cachedMaps = (response?.Data ?? new List<ValorantApiMapDto>())
                    .Where(x => !string.IsNullOrWhiteSpace(x.Uuid))
                    .GroupBy(x => x.Uuid, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
                cachedOn = DateTime.UtcNow;

                return cachedMaps;
            }
            catch (Exception ex) when (ex is ExternalApiException or HttpRequestException)
            {
                // Zones are a nice-to-have on top of an import: a valorant-api.com outage must not fail it.
                // The last good copy is served if there is one; nothing is cached otherwise, so the next import retries.
                return cachedMaps ?? new Dictionary<string, ValorantApiMapDto>(StringComparer.OrdinalIgnoreCase);
            }
            finally
            {
                CacheLock.Release();
            }
        }
    }
}
