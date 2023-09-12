// <copyright file="IFifaGamePlayedBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business.Contracts
{
    using YuGames.DTOs;
    using YuGames.Entities;

    /// <summary>
    /// GamePlayed business interface.
    /// </summary>
    public interface IFifaGamePlayedBusiness
    {
        /// <summary>
        /// Get last games played.
        /// </summary>
        /// <param name="number">Number of data to retrieve.</param>
        /// <returns>List of games played.</returns>
        Task<IEnumerable<FifaGamePlayedDto>> GetLastGamesPlayed(int number);

        /// <summary>
        /// Creates game in database.
        /// </summary>
        /// <param name="createGameDto"><see cref="CreateFifaGameDto"/>.</param>
        /// <returns><see cref="FifaGamePlayed"/>.</returns>
        Task<FifaGamePlayed?> Create(CreateFifaGameDto createGameDto);

        /// <summary>
        /// Get last games played by player.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>List of GamePlayed DTOs.</returns>
        Task<IEnumerable<FifaGamePlayedDto>> GetLastGamesPlayedByPlayerId(int playerId, int limit);

        /// <summary>
        /// Get game by it's ID.
        /// </summary>
        /// <param name="gameId">Game ID.</param>
        /// <returns><see cref="FifaGamePlayedDto" /> if found, null if not found.</returns>
        Task<FifaGamePlayedDto?> GetById(int gameId);

        /// <summary>
        /// Updates game in database.
        /// </summary>
        /// <param name="fifaGame"><see cref="FifaGamePlayed" />.</param>
        /// <returns><see cref="FifaGamePlayed"/> object.</returns>
        Task<FifaGamePlayed?> Update(UpdateGameDto fifaGame);

        /// <summary>
        /// Delete game.
        /// </summary>
        /// <param name="fifaGameId">Fifa Game ID.</param>
        /// <returns>true if success.</returns>
        Task<bool> Delete(int fifaGameId);

        /// <summary>
        /// Search game in database.
        /// </summary>
        /// <param name="limit">Limit (10 by default, 50 max).</param>
        /// <param name="platformId">Platform ID.</param>
        /// <param name="startDate">Start Date.</param>
        /// <param name="endDate">End date.</param>
        /// <returns>List of <see cref="FifaGamePlayedDto"/>.</returns>
        Task<List<FifaGamePlayedDto>> Search(int? limit, int? platformId, DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Gets current season.
        /// </summary>
        /// <returns><see cref="Season" />.</returns>
        Task<Season?> GetCurrentSeason();
    }
}