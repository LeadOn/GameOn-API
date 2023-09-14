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

        /// <summary>
        /// Check player subscription to a tournament.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <param name="playerId">Player ID.</param>
        /// <returns>True if subscribed, false if not.</returns>
        Task<bool> CheckPlayerSubscription(int tournamentId, int playerId);

        /// <summary>
        /// Subscribe to a tournament.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <param name="playerId">Player ID.</param>
        /// <param name="fifaTeamId">Fifa Team ID.</param>
        /// <returns><see cref="TournamentPlayer"/>.</returns>
        Task<TournamentPlayer> Subscribe(int tournamentId, int playerId, int fifaTeamId);

        /// <summary>
        /// Delete tournament.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <returns>True if deleted, false if not.</returns>
        Task<bool> Delete(int tournamentId);
    }
}