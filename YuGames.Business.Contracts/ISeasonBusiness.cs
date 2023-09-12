// <copyright file="ISeasonBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business.Contracts
{
    using YuGames.Entities;

    /// <summary>
    /// Season business interface.
    /// </summary>
    public interface ISeasonBusiness
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
    }
}