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
    }
}