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
        /// Get last 5 games played.
        /// </summary>
        /// <returns>5 last games.</returns>
        Task<IEnumerable<GamePlayedDto>> GetLastFiveGames();
    }
}