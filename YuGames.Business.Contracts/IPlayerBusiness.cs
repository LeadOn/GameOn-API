// <copyright file="IPlayerBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business.Contracts
{
    using YuGames.DTOs;
    using YuGames.Entities;

    /// <summary>
    /// Player business interface.
    /// </summary>
    public interface IPlayerBusiness
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
        /// Gets connected user from database.
        /// </summary>
        /// <param name="player"><see cref="ConnectedPlayerDto"/>Connected user.</param>
        /// <returns><see cref="Player"/> object.</returns>
        Task<Player> GetConnectedUser(ConnectedPlayerDto player);

        /// <summary>
        /// Updates user.
        /// </summary>
        /// <param name="fullName">Full name.</param>
        /// <param name="nickname">Nickname.</param>
        /// <param name="profilePictureUrl">Profile Picture URL.</param>
        /// <param name="keycloakId">Keycloak ID.</param>
        /// <returns><see cref="Player" />.</returns>
        Task<Player> UpdatePlayer(string fullName, string nickname, string profilePictureUrl, string keycloakId);
        
        Task<Player> UpdatePlayerAdmin(UpdatePlayerDto update);

        /// <summary>
        /// Get player stats.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="seasonId">Season ID (default is current).</param>
        /// <returns><see cref="FifaPlayerStatsDto"/>.</returns>
        Task<FifaPlayerStatsDto> GetPlayerStats(int playerId, int? seasonId);
    }
}