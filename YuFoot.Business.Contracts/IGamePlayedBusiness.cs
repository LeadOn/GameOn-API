// <copyright file="IGamePlayedBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Business.Contracts
{
    using YuFoot.DTOs;

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
    }
}