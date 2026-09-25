// <copyright file="ILoLLiveGameCache.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Live.Services
{
    /// <summary>
    /// ILoLLiveGameCache interface. Answers "which of these accounts is in a game right now" without asking
    /// Riot on every incoming request: spectator-v5 only answers one account per call, so the answers are
    /// kept a while and shared by every caller.
    /// </summary>
    public interface ILoLLiveGameCache
    {
        /// <summary>
        /// Gets the last known spectator-v5 answer for each account, asking Riot again only about the
        /// accounts whose answer has expired.
        /// </summary>
        /// <param name="puuids">PUUIDs of the accounts to look up.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>Snapshot by PUUID. An account Riot has never answered about yet (rate limited since
        /// startup) is absent.</returns>
        Task<IReadOnlyDictionary<string, LoLLiveGameSnapshot>> GetActiveGames(IReadOnlyCollection<string> puuids, CancellationToken cancellationToken);
    }
}
