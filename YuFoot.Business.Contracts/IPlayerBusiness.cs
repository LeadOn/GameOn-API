// <copyright file="IPlayerBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>
namespace YuFoot.Business.Contracts
{
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
        Task<Player?> GetPlayerById(int id);

        /// <summary>
        /// Get all players in database.
        /// </summary>
        /// <returns>List of players.</returns>
        Task<IEnumerable<Player>> GetAll();
    }
}