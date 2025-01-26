// <copyright file="IMatchService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Interfaces
{
    using System.Threading;
    using GameOn.External.RiotGames.Models.DTOs;

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

        /// <summary>
        /// Gets game by it's Riot Games ID.
        /// </summary>
        /// <param name="matchId">ID of the game.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns><see cref="MatchDto"/>Match DTO.</returns>
        Task<MatchDto> GetGameById(string matchId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets game's timeline by it's Riot Games ID.
        /// </summary>
        /// <param name="matchId">ID of the game.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns><see cref="TimelineDto"/>Timeline DTO.</returns>
        Task<TimelineDto> GetGameTimelineById(string matchId, CancellationToken cancellationToken);
    }
}
