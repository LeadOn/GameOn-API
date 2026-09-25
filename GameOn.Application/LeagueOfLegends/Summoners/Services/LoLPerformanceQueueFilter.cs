// <copyright file="LoLPerformanceQueueFilter.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Services
{
    /// <summary>
    /// Tells which queues count towards an account's own performance numbers: the champion and role stats of
    /// <c>GET lol/summoner/{id}</c> (<c>GetLeaguePlayerByIdQueryHandler</c>) and the main champion of
    /// <c>GET lol/summoner</c> (<c>GetAllLeaguePlayersQueryHandler</c>). Shared because the second is
    /// documented as "the first entry of the first", and two copies of the list would drift.
    /// </summary>
    /// <remarks>
    /// Not the same list as <c>GetLoLGlobalStatsQueryHandler</c>'s, which also carries the French keywords
    /// ("personnalis", "entraînement") of the custom game queues completed from Community Dragon. Without them,
    /// custom games (queues 3100, 3110, 3140) do count here. Kept as is on purpose: aligning it would change
    /// what <c>GET lol/summoner/{id}</c> already returns.
    /// </remarks>
    public static class LoLPerformanceQueueFilter
    {
        // Matched against LoLQueue.Map + Description (synced from Riot) to keep only games against real
        // opponents.
        private static readonly string[] ExcludedQueueTypeKeywords = { "Co-op", "Bot", "Tutorial", "Custom" };

        /// <summary>
        /// Tells whether games of a queue are left out of an account's performance numbers.
        /// </summary>
        /// <param name="map">The queue's map (<c>LoLQueue.Map</c>).</param>
        /// <param name="description">The queue's description (<c>LoLQueue.Description</c>).</param>
        /// <returns>True when the queue is not played against real opponents (bots, tutorial...).</returns>
        public static bool IsExcluded(string? map, string? description)
        {
            var queueLabel = (map ?? string.Empty) + " " + (description ?? string.Empty);

            return ExcludedQueueTypeKeywords.Any(keyword => queueLabel.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }
}
