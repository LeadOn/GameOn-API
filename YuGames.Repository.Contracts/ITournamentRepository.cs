// <copyright file="ITournamentRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository.Contracts
{
    using System.Linq.Expressions;
    using YuGames.DTOs;
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
        /// Get tournament by ID in database.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <returns><see cref="Tournament"/>.</returns>
        Task<Tournament?> GetById(int id);

        /// <summary>
        /// Gets count.
        /// </summary>
        /// <returns>Count.</returns>
        Task<int> Count();

        /// <summary>
        /// Gets tournament players.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <returns><see cref="List{TournamentPlayerDto}"/> Tournament players.</returns>
        Task<List<TournamentPlayerDto>> GetPlayers(int tournamentId);

        /// <summary>
        /// Update tournament in database.
        /// </summary>
        /// <param name="tournament">Updated <see cref="Tournament"/>.</param>
        /// <returns><see cref="Tournament"/>.</returns>
        Task<Tournament> UpdateTournament(Tournament tournament);
    }
}