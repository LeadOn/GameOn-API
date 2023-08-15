// <copyright file="IGamePlayedRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository.Contracts
{
    using System.Linq.Expressions;
    using YuGames.Entities;

    /// <summary>
    /// GamePlayed repository.
    /// </summary>
    public interface IGamePlayedRepository
    {
        /// <summary>
        /// Get GamePlayed by its ID.
        /// </summary>
        /// <param name="id">GamePlayed ID.</param>
        /// <returns>GamePlay if found, null if not.</returns>
        Task<GamePlayed?> GetById(int id);

        /// <summary>
        /// Search GamePlayed in table.
        /// </summary>
        /// <param name="query">Query to filter in table.</param>
        /// <param name="limit">Number of data to retrieve.</param>
        /// <returns>List of GamePlayed objects.</returns>
        Task<IEnumerable<GamePlayed>> Search(Expression<Func<GamePlayed, bool>> query, int limit);

        /// <summary>
        /// Creates game in database.
        /// </summary>
        /// <param name="game"><see cref="GamePlayed" />.</param>
        /// <returns><see cref="GamePlayed"/> object.</returns>
        Task<GamePlayed> CreateGame(GamePlayed game);
    }
}