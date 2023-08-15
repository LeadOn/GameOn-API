// <copyright file="IFifaTeamBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business.Contracts
{
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
    }
}