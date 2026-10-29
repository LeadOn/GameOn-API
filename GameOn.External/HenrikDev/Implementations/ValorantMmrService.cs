// <copyright file="ValorantMmrService.cs" company="LeadOn's Corp'">
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
    /// <see cref="IValorantMmrService"/> implementation backed by the HenrikDev Valorant API (MMR v3 and MMR History v2).
    /// </summary>
    public class ValorantMmrService : HttpServiceBase, IValorantMmrService
    {
        private const string DefaultBaseUrl = "https://api.henrikdev.xyz";
        private const string DefaultPlatform = "pc";

        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantMmrService"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public ValorantMmrService(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<ValorantMmrDto?> GetMmrByPuuid(string region, string valorantPuuid, CancellationToken cancellationToken)
        {
            var encodedRegion = Uri.EscapeDataString(string.IsNullOrWhiteSpace(region) ? "eu" : region.Trim().ToLowerInvariant());
            var encodedPuuid = Uri.EscapeDataString(valorantPuuid);
            var request = CreateRequest($"{GetBaseUrl()}/valorant/v3/by-puuid/mmr/{encodedRegion}/{DefaultPlatform}/{encodedPuuid}");

            try
            {
                var response = await RunRequest<HenrikDevResponseDto<ValorantMmrDto>>(this.client, request, cancellationToken);
                return response?.Data;
            }
            catch (ExternalApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<ValorantMmrHistoryDto?> GetMmrHistoryByPuuid(string region, string valorantPuuid, CancellationToken cancellationToken)
        {
            var encodedRegion = Uri.EscapeDataString(string.IsNullOrWhiteSpace(region) ? "eu" : region.Trim().ToLowerInvariant());
            var encodedPuuid = Uri.EscapeDataString(valorantPuuid);
            var request = CreateRequest($"{GetBaseUrl()}/valorant/v2/by-puuid/mmr-history/{encodedRegion}/{DefaultPlatform}/{encodedPuuid}");

            try
            {
                var response = await RunRequest<HenrikDevResponseDto<ValorantMmrHistoryDto>>(this.client, request, cancellationToken);
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