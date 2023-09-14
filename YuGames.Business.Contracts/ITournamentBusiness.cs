// <copyright file="ITournamentBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business.Contracts
{
    using YuGames.DTOs;
    using YuGames.Entities;

    /// <summary>
    /// Tournament business interface.
    /// </summary>
    public interface ITournamentBusiness
    {
        /// <summary>
        /// Create tournament.
        /// </summary>
        /// <param name="tournament"><see cref="TournamentDto"/>.</param>
        /// <returns>Created <see cref="Tournament"/>.</returns>
        Task<Tournament> Create(TournamentDto tournament);

        /// <summary>
        /// Get all tournaments in database.
        /// </summary>
        /// <returns>List of Tournaments.</returns>
        Task<List<Tournament>> GetAll();
    }
}