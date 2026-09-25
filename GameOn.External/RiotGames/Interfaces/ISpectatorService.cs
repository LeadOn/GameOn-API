// <copyright file="ISpectatorService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Interfaces
{
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// ISpectatorService interface. Riot's spectator-v5: the games being played right now.
    /// </summary>
    public interface ISpectatorService
    {
        /// <summary>
        /// Gets the game an account is currently playing.
        /// </summary>
        /// <param name="puuid">Account PUUID.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>The game in progress, or null when the account isn't in a game (Riot answers 404).</returns>
        /// <exception cref="Common.Exceptions.ExternalApiException">Riot answered with any other error status (rate limit, stale PUUID, outage...).</exception>
        Task<CurrentGameInfoDto?> GetActiveGame(string puuid, CancellationToken cancellationToken);
    }
}
