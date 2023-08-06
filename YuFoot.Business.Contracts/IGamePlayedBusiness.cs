// <copyright file="IGamePlayedBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Business.Contracts
{
    using YuFoot.DTOs;
    using YuFoot.Entities;

    /// <summary>
    /// GamePlayed business interface.
    /// </summary>
    public interface IGamePlayedBusiness
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
        /// <returns><see cref="GamePlayed"/>.</returns>
        Task<GamePlayed?> CreateGame(CreateGameDto createGameDto);
    }
}