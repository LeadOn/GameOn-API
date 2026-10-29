// <copyright file="IValorantMatchService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.HenrikDev.Models.DTOs;

    /// <summary>
    /// Valorant match history service backed by HenrikDev Valorant v4 endpoints.
    /// </summary>
    public interface IValorantMatchService
    {
        /// <summary>
        /// Retrieves the latest Valorant matches played by a player from their Valorant PUUID.
        /// </summary>
        /// <param name="region">Valorant region/shard (e.g. "eu").</param>
        /// <param name="valorantPuuid">Raw 36-character Valorant PUUID.</param>
        /// <param name="size">Maximum number of matches to return (1 to 10).</param>
        /// <param name="mode">Optional queue/mode filter (e.g. "competitive", "unrated").</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of full Valorant match payloads.</returns>
        Task<List<ValorantMatchDto>> GetLastMatchesByPuuid(string region, string valorantPuuid, int size = 10, string? mode = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single Valorant match by its match UUID.
        /// </summary>
        /// <param name="region">Valorant region/shard (e.g. "eu").</param>
        /// <param name="matchId">Valorant match UUID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Full Valorant match payload, or null when not found.</returns>
        Task<ValorantMatchDto?> GetMatchById(string region, string matchId, CancellationToken cancellationToken = default);
    }
}
