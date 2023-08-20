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
        Task<IEnumerable<GamePlayedDto>> GetLastGamesPlayed(int number);

        /// <summary>
        /// Creates game in database.
        /// </summary>
        /// <param name="createGameDto"><see cref="CreateGameDto"/>.</param>
        /// <returns><see cref="FifaGamePlayed"/>.</returns>
        Task<FifaGamePlayed?> Create(CreateGameDto createGameDto);

        /// <summary>
        /// Get last games played by player.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>List of GamePlayed DTOs.</returns>
        Task<IEnumerable<GamePlayedDto>> GetLastGamesPlayedByPlayerId(int playerId, int limit);

        /// <summary>
        /// Get game by it's ID.
        /// </summary>
        /// <param name="gameId">Game ID.</param>
        /// <returns><see cref="GamePlayedDto" /> if found, null if not found.</returns>
        Task<GamePlayedDto?> GetById(int gameId);

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
    }
}