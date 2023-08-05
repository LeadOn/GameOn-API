// <copyright file="IPlayerRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Repository.Contracts
{
    using YuFoot.DTOs;
    using YuFoot.Entities;

    /// <summary>
    /// Player repository.
    /// </summary>
    public interface IPlayerRepository
    {
        /// <summary>
        /// Get player by its ID.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <returns>Player if found, null if not.</returns>
        Task<Player?> GetPlayerById(int id);

        /// <summary>
        /// Get all players in database.
        /// </summary>
        /// <returns>List of players.</returns>
        Task<IEnumerable<Player>> GetAll();

        /// <summary>
        /// Gets player from database by it's Keycloak ID.
        /// </summary>
        /// <param name="player">Connected player DTO.</param>
        /// <returns><see cref="Player"/> entity.</returns>
        Task<Player> GetPlayerByKeycloakId(ConnectedPlayerDto player);
    }
}