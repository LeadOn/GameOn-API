// <copyright file="ILeagueService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Interfaces
{
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// ILeagueService interface.
    /// </summary>
    public interface ILeagueService
    {
        /// <summary>
        /// Gets League Entries by Summoner's ID.
        /// </summary>
        /// <param name="summonerId">Summoner ID.</param>
        /// <returns><see cref="IEnumerable{LeagueEntryDto}"/>.</returns>
        Task<IEnumerable<LeagueEntryDto>> GetLeagueEntries(string summonerId);
    }
}
