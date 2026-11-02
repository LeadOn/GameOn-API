// <copyright file="ValorantStoredDataService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Implementations
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.Common;
    using GameOn.External.Common.Exceptions;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;

    /// <summary>
    /// <see cref="IValorantStoredDataService"/> implementation (stored-matches v1, stored-mmr-history v2).
    /// </summary>
    public class ValorantStoredDataService : HttpServiceBase, IValorantStoredDataService
    {
        private const string DefaultPlatform = "pc";

        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantStoredDataService"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public ValorantStoredDataService(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<HenrikDevPagedResponseDto<ValorantStoredMatchDto>?> GetStoredMatches(string region, string valorantPuuid, int? size, int? page, CancellationToken cancellationToken)
        {
            var path = $"/valorant/v1/by-puuid/stored-matches/{HenrikDevRequestFactory.Region(region)}/{Uri.EscapeDataString(valorantPuuid)}";

            if (size is > 0)
            {
                path += $"?size={size}&page={Math.Max(page ?? 1, 1)}";
            }

            try
            {
                return await RunRequest<HenrikDevPagedResponseDto<ValorantStoredMatchDto>>(this.client, HenrikDevRequestFactory.Get(path, lowPriority: true), cancellationToken);
            }
            catch (ExternalApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<HenrikDevPagedResponseDto<ValorantMmrHistoryEntryDto>?> GetStoredMmrHistory(string region, string valorantPuuid, int size, int page, CancellationToken cancellationToken)
        {
            var path = $"/valorant/v2/by-puuid/stored-mmr-history/{HenrikDevRequestFactory.Region(region)}/{DefaultPlatform}/{Uri.EscapeDataString(valorantPuuid)}?size={Math.Max(size, 1)}&page={Math.Max(page, 1)}";

            try
            {
                return await RunRequest<HenrikDevPagedResponseDto<ValorantMmrHistoryEntryDto>>(this.client, HenrikDevRequestFactory.Get(path, lowPriority: true), cancellationToken);
            }
            catch (ExternalApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}
