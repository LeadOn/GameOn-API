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
        /// Gets League Entries by its PUUID.
        /// </summary>
        /// <param name="puuid">PUUID ID.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns><see cref="IEnumerable{LeagueEntryDto}"/>.</returns>
        Task<IEnumerable<LeagueEntryDto>> GetLeagueEntries(string puuid, CancellationToken cancellationToken);
    }
}
