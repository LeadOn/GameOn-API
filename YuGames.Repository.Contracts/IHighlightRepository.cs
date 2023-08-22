// <copyright file="IHighlightRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository.Contracts
{
    using YuGames.Entities;

    /// <summary>
    /// Highlight repository.
    /// </summary>
    public interface IHighlightRepository
    {
        /// <summary>
        /// Gets count.
        /// </summary>
        /// <returns>Count.</returns>
        Task<int> Count();

        /// <summary>
        /// Deletes all highlights associated to a given fifa game.
        /// </summary>
        /// <param name="fifaGameId">Fifa Game ID.</param>
        /// <returns>true if success.</returns>
        Task<bool> DeleteAllFifaGame(int fifaGameId);

        /// <summary>
        /// Returns all highlights in database.
        /// </summary>
        /// <returns>List of Highlights.</returns>
        Task<List<Highlight>> GetAll();
    }
}