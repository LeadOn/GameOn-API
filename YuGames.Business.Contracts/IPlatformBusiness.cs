// <copyright file="IPlatformBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business.Contracts
{
    using YuGames.Entities;

    /// <summary>
    /// Platform business interface.
    /// </summary>
    public interface IPlatformBusiness
    {
        /// <summary>
        /// Get all platforms.
        /// </summary>
        /// <returns>List of <see cref="Platform"/>.</returns>
        Task<IEnumerable<Platform>> GetAll();

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
    }
}