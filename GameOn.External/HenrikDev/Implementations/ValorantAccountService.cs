// <copyright file="ValorantAccountService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Implementations
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.Common;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;

    /// <summary>
    /// <see cref="IValorantAccountService"/> implementation backed by the HenrikDev Valorant API (v2).
    /// </summary>
    public class ValorantAccountService : HttpServiceBase, IValorantAccountService
    {
        private const string DefaultBaseUrl = "https://api.henrikdev.xyz";

        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantAccountService"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public ValorantAccountService(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<ValorantAccountDto?> GetAccountByRiotId(string gameName, string tagLine, CancellationToken cancellationToken)
        {
            var encodedName = Uri.EscapeDataString(gameName);
            var encodedTag = Uri.EscapeDataString(tagLine);
            var request = CreateRequest($"{GetBaseUrl()}/valorant/v2/account/{encodedName}/{encodedTag}");
            var response = await RunRequest<HenrikDevResponseDto<ValorantAccountDto>>(this.client, request, cancellationToken);
            return response?.Data;
        }

        /// <inheritdoc/>
        public async Task<ValorantAccountDto?> GetAccountByPuuid(string valorantPuuid, CancellationToken cancellationToken)
        {
            var encodedPuuid = Uri.EscapeDataString(valorantPuuid);
            var request = CreateRequest($"{GetBaseUrl()}/valorant/v2/by-puuid/account/{encodedPuuid}");
            var response = await RunRequest<HenrikDevResponseDto<ValorantAccountDto>>(this.client, request, cancellationToken);
            return response?.Data;
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

            // HenrikDev expects the raw API key (HDEV-...) in the Authorization header without a scheme prefix.
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                request.Headers.TryAddWithoutValidation("Authorization", apiKey.Trim());
            }

            return request;
        }
    }
}