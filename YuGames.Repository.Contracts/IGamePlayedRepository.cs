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
        Task<FifaGamePlayed?> GetById(int id);

        /// <summary>
        /// Search GamePlayed in table.
        /// </summary>
        /// <param name="query">Query to filter in table.</param>
        /// <param name="limit">Number of data to retrieve.</param>
        /// <returns>List of GamePlayed objects.</returns>
        Task<IEnumerable<FifaGamePlayed>> Search(Expression<Func<FifaGamePlayed, bool>> query, int limit);

        /// <summary>
        /// Creates game in database.
        /// </summary>
        /// <param name="fifaGame"><see cref="FifaGamePlayed" />.</param>
        /// <returns><see cref="FifaGamePlayed"/> object.</returns>
        Task<FifaGamePlayed> Create(FifaGamePlayed fifaGame);

        /// <summary>
        /// Updates game in database.
        /// </summary>
        /// <param name="fifaGame"><see cref="FifaGamePlayed" />.</param>
        /// <returns><see cref="FifaGamePlayed"/> object.</returns>
        Task<FifaGamePlayed?> Update(FifaGamePlayed fifaGame);
    }
}