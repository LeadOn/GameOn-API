// <copyright file="MatchV5Service.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.Common;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// MatchV5Service class.
    /// </summary>
    public class MatchV5Service : HttpServiceBase, IMatchService
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchV5Service"/> class.
        /// </summary>
        /// <param name="client">HTTP Client.</param>
        public MatchV5Service(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetLastGamesPlayed(string puuid, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{Environment.GetEnvironmentVariable("RIOT_GAMES_ACCOUNT_API_ROUTE")}/lol/match/v5/matches/by-puuid/{puuid}/ids?api_key={Environment.GetEnvironmentVariable("RIOT_GAMES_API_KEY")}");
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await RunRequest<IEnumerable<string>>(this.client, request, cancellationToken);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
        }

        /// <inheritdoc />
        public async Task<MatchDto> GetGameById(string matchId, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{Environment.GetEnvironmentVariable("RIOT_GAMES_ACCOUNT_API_ROUTE")}/lol/match/v5/matches/{matchId}?api_key={Environment.GetEnvironmentVariable("RIOT_GAMES_API_KEY")}");
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await RunRequest<MatchDto>(this.client, request, cancellationToken);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
        }

        /// <inheritdoc />
        public async Task<TimelineDto> GetGameTimelineById(string matchId, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{Environment.GetEnvironmentVariable("RIOT_GAMES_ACCOUNT_API_ROUTE")}/lol/match/v5/matches/{matchId}/timeline?api_key={Environment.GetEnvironmentVariable("RIOT_GAMES_API_KEY")}");
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await RunRequest<TimelineDto>(this.client, request, cancellationToken);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
        }
    }
}
