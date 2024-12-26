// <copyright file="ISummonerService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Interfaces
{
    using System.Threading;
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// SummonerService interface.
    /// </summary>
    public interface ISummonerService
    {
        /// <summary>
        /// Gets Riot Games summoner via PUUID.
        /// </summary>
        /// <param name="puuid">PUUID of the player.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AccountDto"/>.</returns>
        Task<SummonerDto> GetSummonerByPuuid(string puuid, CancellationToken cancellationToken);
    }
}
