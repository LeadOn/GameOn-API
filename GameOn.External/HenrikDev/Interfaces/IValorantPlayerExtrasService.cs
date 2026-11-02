// <copyright file="IValorantPlayerExtrasService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.HenrikDev.Models.DTOs;

    /// <summary>
    /// Player data that changes slowly enough to be read once a day: accolades, agent mastery and Premier team.
    /// Every call is low priority.
    /// </summary>
    public interface IValorantPlayerExtrasService
    {
        /// <summary>
        /// Gets a player's accolades (accolades v1).
        /// </summary>
        /// <param name="region">Region/affinity (e.g. eu).</param>
        /// <param name="valorantPuuid">Player's Valorant PUUID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The accolades, or null when the player is unknown.</returns>
        Task<ValorantAccoladesDto?> GetAccolades(string region, string valorantPuuid, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a player's mastery of every agent (agent-mastery v1).
        /// </summary>
        /// <param name="region">Region/affinity (e.g. eu).</param>
        /// <param name="valorantPuuid">Player's Valorant PUUID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The masteries, or null when the player is unknown.</returns>
        Task<ValorantAgentMasteryListDto?> GetAgentMastery(string region, string valorantPuuid, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the Premier team a player belongs to (premier v2).
        /// </summary>
        /// <param name="region">Region/affinity (e.g. eu).</param>
        /// <param name="valorantPuuid">Player's Valorant PUUID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The team, or null when the player has none.</returns>
        Task<ValorantPremierTeamDto?> GetPremierTeamByPlayer(string region, string valorantPuuid, CancellationToken cancellationToken);
    }
}
