// <copyright file="ValorantPlayerExtrasService.cs" company="LeadOn's Corp'">
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
    /// <see cref="IValorantPlayerExtrasService"/> implementation (accolades v1, agent-mastery v1, premier v2).
    /// </summary>
    public class ValorantPlayerExtrasService : HttpServiceBase, IValorantPlayerExtrasService
    {
        private const string DefaultPlatform = "pc";

        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantPlayerExtrasService"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public ValorantPlayerExtrasService(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public Task<ValorantAccoladesDto?> GetAccolades(string region, string valorantPuuid, CancellationToken cancellationToken) =>
            this.GetOrNull<ValorantAccoladesDto>($"/valorant/v1/by-puuid/accolades/{HenrikDevRequestFactory.Region(region)}/{DefaultPlatform}/{Uri.EscapeDataString(valorantPuuid)}", cancellationToken);

        /// <inheritdoc/>
        public Task<ValorantAgentMasteryListDto?> GetAgentMastery(string region, string valorantPuuid, CancellationToken cancellationToken) =>
            this.GetOrNull<ValorantAgentMasteryListDto>($"/valorant/v1/by-puuid/agent-mastery/{HenrikDevRequestFactory.Region(region)}/{DefaultPlatform}/{Uri.EscapeDataString(valorantPuuid)}", cancellationToken);

        /// <inheritdoc/>
        public Task<ValorantPremierTeamDto?> GetPremierTeamByPlayer(string region, string valorantPuuid, CancellationToken cancellationToken) =>
            this.GetOrNull<ValorantPremierTeamDto>($"/valorant/v2/premier/players/{HenrikDevRequestFactory.Region(region)}/{Uri.EscapeDataString(valorantPuuid)}", cancellationToken);

        private async Task<T?> GetOrNull<T>(string path, CancellationToken cancellationToken)
            where T : class
        {
            try
            {
                var response = await RunRequest<HenrikDevResponseDto<T>>(this.client, HenrikDevRequestFactory.Get(path, lowPriority: true), cancellationToken);
                return response?.Data;
            }
            catch (ExternalApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}
