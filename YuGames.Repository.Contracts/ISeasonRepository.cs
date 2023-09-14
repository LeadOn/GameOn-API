// <copyright file="ISeasonRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository.Contracts
{
    using YuGames.Entities;

    /// <summary>
    /// Season repository.
    /// </summary>
    public interface ISeasonRepository
    {
        /// <summary>
        /// Get current season.
        /// </summary>
        /// <returns><see cref="Season"/>.</returns>
        Task<Season?> GetCurrent();

        /// <summary>
        /// Get all seasons.
        /// </summary>
        /// <returns><see cref="List{Season}"/>.</returns>
        Task<List<Season>> GetAll();

        /// <summary>
        /// Gets count.
        /// </summary>
        /// <returns>Count.</returns>
        Task<int> Count();
    }
}