// <copyright file="ValorantMatchService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.Common;
    using GameOn.External.Common.Exceptions;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;

    /// <summary>
    /// <see cref="IValorantMatchService"/> implementation backed by the HenrikDev Valorant v4 API.
    /// </summary>
    public class ValorantMatchService : HttpServiceBase, IValorantMatchService
    {
        private const string DefaultBaseUrl = "https://api.henrikdev.xyz";
        private const string DefaultPlatform = "pc";

        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantMatchService"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public ValorantMatchService(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<List<ValorantMatchDto>> GetLastMatchesByPuuid(string region, string valorantPuuid, int size = 10, string? mode = null, CancellationToken cancellationToken = default)
        {
            var encodedRegion = Uri.EscapeDataString(string.IsNullOrWhiteSpace(region) ? "eu" : region.Trim().ToLowerInvariant());
            var encodedPuuid = Uri.EscapeDataString(valorantPuuid);
            var boundedSize = Math.Clamp(size, 1, 10);
            var url = $"{GetBaseUrl()}/valorant/v4/by-puuid/matches/{encodedRegion}/{DefaultPlatform}/{encodedPuuid}?size={boundedSize}";

            if (!string.IsNullOrWhiteSpace(mode))
            {
                url += $"&mode={Uri.EscapeDataString(mode.Trim().ToLowerInvariant())}";
            }

            var request = CreateRequest(url);

            try
            {
                var response = await RunRequest<HenrikDevResponseDto<List<ValorantMatchDto>>>(this.client, request, cancellationToken);
                return response?.Data ?? new List<ValorantMatchDto>();
            }
            catch (ExternalApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<ValorantMatchDto>();
            }
        }

        /// <inheritdoc/>
        public async Task<ValorantMatchDto?> GetMatchById(string region, string matchId, CancellationToken cancellationToken = default)
        {
            var encodedRegion = Uri.EscapeDataString(string.IsNullOrWhiteSpace(region) ? "eu" : region.Trim().ToLowerInvariant());
            var encodedMatchId = Uri.EscapeDataString(matchId);
            var request = CreateRequest($"{GetBaseUrl()}/valorant/v4/match/{encodedRegion}/{encodedMatchId}");

            try
            {
                var response = await RunRequest<HenrikDevResponseDto<ValorantMatchDto>>(this.client, request, cancellationToken);
                return response?.Data;
            }
            catch (ExternalApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        private static string GetBaseUrl()
        {
            var configured = Environment.GetEnvironmentVariable("HENRIKDEV_API_BASE_URL");
            return string.IsNullOrWhiteSpace(configured) ? DefaultBaseUrl : configured.Trim().TrimEnd('/');
        }

        private static HttpRequestMessage CreateRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var apiKey = Environment.GetEnvironmentVariable("HENRIKDEV_API_KEY");

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                request.Headers.TryAddWithoutValidation("Authorization", apiKey.Trim());
            }

            return request;
        }
    }
}
