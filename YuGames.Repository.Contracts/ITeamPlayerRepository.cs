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
        Task<IEnumerable<TeamPlayer>> GetTeamPlayersByGameId(int gamePlayedId);

        /// <summary>
        /// Get TeamPlayer by Player ID.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <returns>List of <see cref="TeamPlayer"/>.</returns>
        Task<IEnumerable<TeamPlayer>> GetTeamPlayerByPlayerId(int playerId);

        /// <summary>
        /// Create team player.
        /// </summary>
        /// <param name="teamPlayer">Team player.</param>
        /// <returns><see cref="TeamPlayer"/>.</returns>
        Task<TeamPlayer> CreateTeamPlayer(TeamPlayer teamPlayer);

        /// <summary>
        /// Search TeamPlayer in table.
        /// </summary>
        /// <param name="query">Query to filter in table.</param>
        /// <param name="limit">Number of data to retrieve.</param>
        /// <returns>List of TeamPlayer objects.</returns>
        Task<IEnumerable<TeamPlayer>> Search(Expression<Func<TeamPlayer, bool>> query, int limit);
    }
}