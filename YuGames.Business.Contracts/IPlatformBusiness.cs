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
    }
}