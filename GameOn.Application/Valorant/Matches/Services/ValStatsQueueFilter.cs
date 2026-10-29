// <copyright file="ValStatsQueueFilter.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Services
{
    using System.Linq;
    using GameOn.Domain;

    /// <summary>
    /// Tells which Valorant games count towards stats and awards: the profile recap of <c>GET val/player/{id}</c>,
    /// the crew records of <c>GET val/stats/global</c> and the home page of <c>GET val/home</c>. Shared so that
    /// the three never drift apart.
    /// </summary>
    public static class ValStatsQueueFilter
    {
        /// <summary>
        /// HenrikDev queue ID of the ranked queue, the only one that moves RR.
        /// </summary>
        public const string CompetitiveQueueId = "competitive";

        // Queues left out of every stat. Customs aren't played against real opponents (same reason as the LoL
        // keyword exclusion), and both deathmatch modes ("hurm" is HenrikDev's ID for Team Deathmatch) have no
        // rounds, no spike and no economy, so ACS, ADR, KAST or first kills mean nothing there. "unknown" is
        // what UpsertValorantMatchesCommandHandler stores when HenrikDev gave no queue at all.
        private static readonly string[] ExcludedQueueIds = { "custom", "deathmatch", "hurm", "unknown" };

        /// <summary>
        /// Keeps the participations of games that count: queue known and not excluded, and not a remake.
        /// </summary>
        /// <param name="participants">Participations query.</param>
        /// <returns>The query, restricted to counted games.</returns>
        public static IQueryable<ValGameParticipant> CountedGamesOnly(this IQueryable<ValGameParticipant> participants)
        {
            return participants.Where(x => !x.Game.IsRemake
                && x.Game.QueueId != null
                && !ExcludedQueueIds.Contains(x.Game.QueueId));
        }
    }
}
