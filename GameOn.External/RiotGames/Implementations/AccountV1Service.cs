// <copyright file="AccountV1Service.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Implementations
{
    using GameOn.External.Common;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;
    using Newtonsoft.Json;

    /// <summary>
    /// AccountV1Service class.
    /// </summary>
    public class AccountV1Service : HttpServiceBase, IAccountService
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountV1Service"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public AccountV1Service(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<AccountDto> GetAccountPuuid(string tagLine, string nickname, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{Environment.GetEnvironmentVariable("RIOT_GAMES_ACCOUNT_API_ROUTE")}/riot/account/v1/accounts/by-riot-id/{nickname}/{tagLine}?api_key={Environment.GetEnvironmentVariable("RIOT_GAMES_API_KEY")}");
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await RunRequest<AccountDto>(this.client, request, cancellationToken);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null
        }
    }
}
