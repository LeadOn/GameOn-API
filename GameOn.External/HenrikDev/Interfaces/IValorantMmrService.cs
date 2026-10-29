// <copyright file="IValorantMmrService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.HenrikDev.Models.DTOs;

    /// <summary>
    /// Interface for fetching Valorant MMR and competitive rank history from the HenrikDev API.
    /// </summary>
    public interface IValorantMmrService
    {
        /// <summary>
        /// Retrieves a player's current and peak MMR by their Valorant PUUID.
        /// </summary>
        /// <param name="region">Valorant region affinity (e.g. eu, na, ap, kr, latam, br).</param>
        /// <param name="valorantPuuid">36-char Valorant PUUID.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>The resolved <see cref="ValorantMmrDto"/>, or null if none came back.</returns>
        Task<ValorantMmrDto?> GetMmrByPuuid(string region, string valorantPuuid, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a player's recent competitive match MMR history by their Valorant PUUID.
        /// </summary>
        /// <param name="region">Valorant region affinity (e.g. eu, na, ap, kr, latam, br).</param>
        /// <param name="valorantPuuid">36-char Valorant PUUID.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>The resolved <see cref="ValorantMmrHistoryDto"/>, or null if none came back.</returns>
        Task<ValorantMmrHistoryDto?> GetMmrHistoryByPuuid(string region, string valorantPuuid, CancellationToken cancellationToken);
    }
}