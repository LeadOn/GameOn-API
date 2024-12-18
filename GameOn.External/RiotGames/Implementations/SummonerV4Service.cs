// <copyright file="SummonerV4Service.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Implementations
{
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;
    using Newtonsoft.Json;

    /// <summary>
    /// SummonerV4Service class.
    /// </summary>
    public class SummonerV4Service : ISummonerService
    {
        private HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SummonerV4Service"/> class.
        /// </summary>
        public SummonerV4Service()
        {
            this.httpClient = new HttpClient();
        }

        /// <inheritdoc/>
        public async Task<SummonerDto> GetSummonerByPuuid(string puuid)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://{Environment.GetEnvironmentVariable("RIOT_GAMES_SUMMONER_API_ROUTE")}/lol/summoner/v4/summoners/by-puuid/{puuid}?api_key={Environment.GetEnvironmentVariable("RIOT_GAMES_API_KEY")}");
                var response = await this.httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
                    return JsonConvert.DeserializeObject<SummonerDto>(responseBody);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
