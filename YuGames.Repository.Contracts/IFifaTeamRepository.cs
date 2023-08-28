// <copyright file="IFifaTeamRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository.Contracts
{
    using YuGames.DTOs;
    using YuGames.Entities;

    /// <summary>
    /// Fifa Team repository.
    /// </summary>
    public interface IFifaTeamRepository
    {
        /// <summary>
        /// Get all teams in database.
        /// </summary>
        /// <returns>List of Teams.</returns>
        Task<IEnumerable<FifaTeam>> GetAll();

        /// <summary>
        /// Gets FIFA Team by ID.
        /// </summary>
        /// <param name="id">Team ID.</param>
        /// <returns>Team if found, null if not.</returns>
        Task<FifaTeam?> GetById(int id);

        /// <summary>
        /// Get Most Played Teams of a player.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="numberOfTeams">Number of teams to retrieve.</param>
        /// <returns><see cref="List{TopTeamStatDto}"/>.</returns>
        Task<List<TopTeamStatDto>> GetMostPlayedTeams(int playerId, int numberOfTeams);
    }
}