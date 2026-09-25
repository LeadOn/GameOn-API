// <copyright file="SpectatorV5Service.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Implementations
{
    using System.Net;
    using GameOn.External.Common;
    using GameOn.External.Common.Exceptions;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// SpectatorV5Service class.
    /// </summary>
    public class SpectatorV5Service : HttpServiceBase, ISpectatorService
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectatorV5Service"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public SpectatorV5Service(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<CurrentGameInfoDto?> GetActiveGame(string puuid, CancellationToken cancellationToken)
        {
            // Platform-routed like summoner-v4 and league-v4, hence the same host.
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{Environment.GetEnvironmentVariable("RIOT_GAMES_SUMMONER_API_ROUTE")}/lol/spectator/v5/active-games/by-summoner/{puuid}?api_key={Environment.GetEnvironmentVariable("RIOT_GAMES_API_KEY")}");

            try
            {
                return await RunRequest<CurrentGameInfoDto>(this.client, request, cancellationToken);
            }
            catch (ExternalApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // This is how spectator-v5 says "not in a game", which is the normal answer for almost every
                // account at any given time: an answer, not a failure.
                return null;
            }
        }
    }
}
