// <copyright file="IPlatformRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Repository.Contracts
{
    using YuFoot.Entities;

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
    }
}