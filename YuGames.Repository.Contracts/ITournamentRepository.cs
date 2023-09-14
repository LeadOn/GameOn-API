// <copyright file="ITournamentRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository.Contracts
{
    using System.Linq.Expressions;
    using YuGames.Entities;

    /// <summary>
    /// Tournament repository.
    /// </summary>
    public interface ITournamentRepository
    {
        /// <summary>
        /// Create tournament.
        /// </summary>
        /// <param name="tournament"><see cref="Tournament"/>.</param>
        /// <returns>Created <see cref="Tournament"/>.</returns>
        Task<Tournament> Create(Tournament tournament);

        /// <summary>
        /// Get all tournaments in database.
        /// </summary>
        /// <returns>List of Tournaments.</returns>
        Task<List<Tournament>> GetAll();

        /// <summary>
        /// Gets count.
        /// </summary>
        /// <returns>Count.</returns>
        Task<int> Count();
    }
}