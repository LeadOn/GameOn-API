// <copyright file="IMatchService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Interfaces
{
    using System.Threading;

    /// <summary>
    /// IMatchService interface.
    /// </summary>
    public interface IMatchService
    {
        /// <summary>
        /// Gets last games played by user.
        /// </summary>
        /// <param name="puuid">PUUID of the player.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>List of game IDs.</returns>
        Task<IEnumerable<string>> GetLastGamesPlayed(string puuid, CancellationToken cancellationToken);
    }
}
