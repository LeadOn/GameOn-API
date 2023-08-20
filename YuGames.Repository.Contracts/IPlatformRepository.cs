// <copyright file="IPlatformRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository.Contracts
{
    using YuGames.Entities;

    /// <summary>
    /// Platform repository.
    /// </summary>
    public interface IPlatformRepository
    {
        /// <summary>
        /// Get all platforms.
        /// </summary>
        /// <returns>List of <see cref="Platform"/>.</returns>
        Task<IEnumerable<Platform>> GetAll();

        /// <summary>
        /// Get platform by ID.
        /// </summary>
        /// <param name="id">Platform ID.</param>
        /// <returns><see cref="Platform"/>.</returns>
        Task<Platform?> GetById(int id);

        /// <summary>
        /// Creates platform in database.
        /// </summary>
        /// <param name="platform"><see cref="Platform" /> to create.</param>
        /// <returns>Created platform / Null if error happened.</returns>
        Task<Platform?> Create(Platform platform);

        /// <summary>
        /// Update platform in database.
        /// </summary>
        /// <param name="platform">Updated platform.</param>
        /// <returns>Updated platform if success, null if error.</returns>
        Task<Platform?> Update(Platform platform);

        /// <summary>
        /// Gets count.
        /// </summary>
        /// <returns>Count.</returns>
        Task<int> Count();
    }
}