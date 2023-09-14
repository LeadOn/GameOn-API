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

        /// <summary>
        /// Get tournament by ID in database.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <returns><see cref="Tournament"/>.</returns>
        Task<TournamentDto?> GetById(int id);

        /// <summary>
        /// Update tournament in database.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <param name="tournament">Updated <see cref="Tournament"/>.</param>
        /// <returns><see cref="Tournament"/>.</returns>
        Task<Tournament> UpdateTournament(int id, TournamentDto tournament);

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