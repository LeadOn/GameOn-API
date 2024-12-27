// <copyright file="SummonerV4Service.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Implementations
{
    using GameOn.External.Common;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// SummonerV4Service class.
    /// </summary>
    public class SummonerV4Service : HttpServiceBase, ISummonerService
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SummonerV4Service"/> class.
        /// </summary>
        /// <param name="client">HTTP Client.</param>
        public SummonerV4Service(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<SummonerDto> GetSummonerByPuuid(string puuid, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{Environment.GetEnvironmentVariable("RIOT_GAMES_SUMMONER_API_ROUTE")}/lol/summoner/v4/summoners/by-puuid/{puuid}?api_key={Environment.GetEnvironmentVariable("RIOT_GAMES_API_KEY")}");
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await RunRequest<SummonerDto>(this.client, request, cancellationToken);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
        }
    }
}
