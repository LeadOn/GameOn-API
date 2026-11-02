// <copyright file="IValorantStoredDataService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.HenrikDev.Models.DTOs;

    /// <summary>
    /// HenrikDev's stored data: the match and MMR history HenrikDev has accumulated, deeper than Riot's recent
    /// history. Every call is low priority: it only serves the backfill.
    /// </summary>
    public interface IValorantStoredDataService
    {
        /// <summary>
        /// Gets a page of a player's stored matches, newest first.
        /// </summary>
        /// <param name="region">Region/affinity (e.g. eu).</param>
        /// <param name="valorantPuuid">Player's Valorant PUUID.</param>
        /// <param name="size">Page size, null for every stored match in one call.</param>
        /// <param name="page">1-based page, used with <paramref name="size"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The page, or null when the player is unknown.</returns>
        Task<HenrikDevPagedResponseDto<ValorantStoredMatchDto>?> GetStoredMatches(string region, string valorantPuuid, int? size, int? page, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a page of a player's stored MMR history, newest first.
        /// </summary>
        /// <param name="region">Region/affinity (e.g. eu).</param>
        /// <param name="valorantPuuid">Player's Valorant PUUID.</param>
        /// <param name="size">Page size.</param>
        /// <param name="page">1-based page.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The page, or null when the player is unknown.</returns>
        Task<HenrikDevPagedResponseDto<ValorantMmrHistoryEntryDto>?> GetStoredMmrHistory(string region, string valorantPuuid, int size, int page, CancellationToken cancellationToken);
    }
}
