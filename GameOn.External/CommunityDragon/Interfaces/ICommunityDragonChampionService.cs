// <copyright file="ICommunityDragonChampionService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.CommunityDragon.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// ICommunityDragonChampionService interface.
    /// </summary>
    public interface ICommunityDragonChampionService
    {
        /// <summary>
        /// Gets the champion alias (the name used by match-v5, e.g. MonkeyKing) of every champion,
        /// keyed by champion ID. Needed by the sources that only send a champion ID, like the
        /// League client's match history.
        /// </summary>
        /// <param name="cancellationToken">Token to stop all async execution.</param>
        /// <returns>Champion aliases, by champion ID.</returns>
        Task<IReadOnlyDictionary<int, string>> GetChampionNamesById(CancellationToken cancellationToken);
    }
}
