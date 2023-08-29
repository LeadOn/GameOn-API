// <copyright file="IHighlightBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business.Contracts
{
    using YuGames.Entities;

    /// <summary>
    /// Highlight business interface.
    /// </summary>
    public interface IHighlightBusiness
    {
        /// <summary>
        /// Get all highlights in database.
        /// </summary>
        /// <returns>List of Highlights.</returns>
        Task<IEnumerable<Highlight>> GetAll();

        /// <summary>
        /// Create highlight in database.
        /// </summary>
        /// <param name="name">Name of the highlight.</param>
        /// <param name="description">Description of the highlight.</param>
        /// <param name="playerId">Player ID (person who creates).</param>
        /// <param name="fifaGameId">FIFA Game ID.</param>
        /// <param name="externalUrl">External URL.</param>
        /// <returns><see cref="Highlight"/> created.</returns>
        Task<Highlight> Create(string name, string? description, int playerId, int fifaGameId, string? externalUrl);
    }
}