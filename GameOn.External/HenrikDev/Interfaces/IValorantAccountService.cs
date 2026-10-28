// <copyright file="IValorantAccountService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.HenrikDev.Models.DTOs;

    /// <summary>
    /// Interface for fetching Valorant account information from the HenrikDev API.
    /// </summary>
    public interface IValorantAccountService
    {
        /// <summary>
        /// Retrieves a Valorant account by its Riot ID (<paramref name="gameName"/> and <paramref name="tagLine"/>).
        /// </summary>
        /// <param name="gameName">Riot ID game name.</param>
        /// <param name="tagLine">Riot ID tag line.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>The resolved <see cref="ValorantAccountDto"/>, or null if none came back.</returns>
        Task<ValorantAccountDto?> GetAccountByRiotId(string gameName, string tagLine, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a Valorant account by its Valorant PUUID.
        /// </summary>
        /// <param name="valorantPuuid">36-char Valorant PUUID.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>The resolved <see cref="ValorantAccountDto"/>, or null if none came back.</returns>
        Task<ValorantAccountDto?> GetAccountByPuuid(string valorantPuuid, CancellationToken cancellationToken);
    }
}