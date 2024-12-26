// <copyright file="LeagueV4Service.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Implementations
{
    using GameOn.External.Common;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// LeagueV4Service class.
    /// </summary>
    public class LeagueV4Service : HttpServiceBase, ILeagueService
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeagueV4Service"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public LeagueV4Service(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LeagueEntryDto>> GetLeagueEntries(string summonerId, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{Environment.GetEnvironmentVariable("RIOT_GAMES_SUMMONER_API_ROUTE")}/lol/league/v4/entries/by-summoner/{summonerId}?api_key={Environment.GetEnvironmentVariable("RIOT_GAMES_API_KEY")}");
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await RunRequest<IEnumerable<LeagueEntryDto>>(this.client, request, cancellationToken);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null
        }
    }
}
