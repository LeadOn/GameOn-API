// <copyright file="IFifaTeamBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business.Contracts
{
    using YuGames.DTOs;
    using YuGames.Entities;

    /// <summary>
    /// Fifa Team business interface.
    /// </summary>
    public interface IFifaTeamBusiness
    {
        /// <summary>
        /// Get all teams in database.
        /// </summary>
        /// <returns>List of teams.</returns>
        Task<IEnumerable<FifaTeam>> GetAll();

        /// <summary>
        /// Get Most Played Teams of a player.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="numberOfTeams">Number of teams to retrieve.</param>
        /// <param name="seasonId">Season ID.</param>
        /// <returns><see cref="List{TopTeamStatDto}"/>.</returns>
        Task<List<TopTeamStatDto>> GetMostPlayedTeams(int playerId, int numberOfTeams, int seasonId);

        /// <summary>
        /// Get Most wins Teams of a player.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="numberOfTeams">Number of teams to retrieve.</param>
        /// <param name="seasonId">Season ID.</param>
        /// <returns><see cref="List{TopTeamStatDto}"/>.</returns>
        Task<List<TopTeamStatDto>> GetMostWinsTeams(int playerId, int numberOfTeams, int seasonId);

        /// <summary>
        /// Get Most Losses Teams of a player.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="numberOfTeams">Number of teams to retrieve.</param>
        /// <param name="seasonId">Season ID.</param>
        /// <returns><see cref="List{TopTeamStatDto}"/>.</returns>
        Task<List<TopTeamStatDto>> GetMostLossesTeams(int playerId, int numberOfTeams, int seasonId);

        /// <summary>
        /// Gets FIFA Team by ID.
        /// </summary>
        /// <param name="id">Team ID.</param>
        /// <returns>Team if found, null if not.</returns>
        Task<FifaTeam?> GetById(int id);
    }
}