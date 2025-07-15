// <copyright file="INetworkStorage.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Interfaces
{
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// AccountService interface.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Gets Riot Games account PUUID via Tagline / Game name.
        /// </summary>
        /// <param name="tagLine">Tag line (example: EUW).</param>
        /// <param name="nickname">Nickname.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AccountDto"/>.</returns>
        Task<AccountDto> GetAccountPuuid(string tagLine, string nickname, CancellationToken cancellationToken);
    }
}
