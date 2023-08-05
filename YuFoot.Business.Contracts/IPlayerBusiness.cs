// <copyright file="IPlayerBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>
namespace YuFoot.Business.Contracts
{
    using YuFoot.DTOs;
    using YuFoot.Entities;

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
        Task<PlayerDto?> GetPlayerById(int id);

        /// <summary>
        /// Get all players in database.
        /// </summary>
        /// <returns>List of players.</returns>
        Task<IEnumerable<Player>> GetAll();

        /// <summary>
        /// Gets connected user from database.
        /// </summary>
        /// <param name="player"><see cref="ConnectedPlayerDto"/>Connected user.</param>
        /// <returns><see cref="PlayerDto"/> object.</returns>
        Task<PlayerDto> GetConnectedUser(ConnectedPlayerDto player);

        /// <summary>
        /// Updates user.
        /// </summary>
        /// <param name="fullName">Full name.</param>
        /// <param name="nickname">Nickname.</param>
        /// <param name="profilePictureUrl">Profile Picture URL.</param>
        /// <param name="keycloakId">Keycloak ID.</param>
        /// <returns><see cref="PlayerDto" />.</returns>
        Task<PlayerDto> UpdatePlayer(string fullName, string nickname, string profilePictureUrl, string keycloakId);
    }
}