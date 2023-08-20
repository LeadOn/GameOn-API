// <copyright file="ITeamPlayerRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository.Contracts
{
    using System.Linq.Expressions;
    using YuGames.Entities;

    /// <summary>
    /// Team Player repository.
    /// </summary>
    public interface ITeamPlayerRepository
    {
        /// <summary>
        /// Get Team players by game ID.
        /// </summary>
        /// <param name="gamePlayedId">GamePlayed ID.</param>
        /// <returns>List of Team Players.</returns>
        Task<IEnumerable<FifaTeamPlayer>> GetTeamPlayersByGameId(int gamePlayedId);

        /// <summary>
        /// Get TeamPlayer by Player ID.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <returns>List of <see cref="FifaTeamPlayer"/>.</returns>
        Task<IEnumerable<FifaTeamPlayer>> GetTeamPlayerByPlayerId(int playerId);

        /// <summary>
        /// Create team player.
        /// </summary>
        /// <param name="fifaTeamPlayer">Team player.</param>
        /// <returns><see cref="FifaTeamPlayer"/>.</returns>
        Task<FifaTeamPlayer> CreateTeamPlayer(FifaTeamPlayer fifaTeamPlayer);

        /// <summary>
        /// Search TeamPlayer in table.
        /// </summary>
        /// <param name="query">Query to filter in table.</param>
        /// <param name="limit">Number of data to retrieve.</param>
        /// <returns>List of TeamPlayer objects.</returns>
        Task<IEnumerable<FifaTeamPlayer>> Search(Expression<Func<FifaTeamPlayer, bool>> query, int limit);

        /// <summary>
        /// Deletes all team players associated to a given fifa game.
        /// </summary>
        /// <param name="fifaGameId">Fifa Game ID.</param>
        /// <returns>true if success.</returns>
        Task<bool> DeleteAllFifaGame(int fifaGameId);
    }
}