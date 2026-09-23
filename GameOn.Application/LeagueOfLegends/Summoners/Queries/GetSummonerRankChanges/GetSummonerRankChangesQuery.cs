// <copyright file="GetSummonerRankChangesQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetSummonerRankChanges
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// GetSummonerRankChangesQuery class. A player's last ranked games with the LP each one won or lost,
    /// the per-game counterpart of <c>GetSummonerRankHistoryQuery</c>.
    /// </summary>
    public class GetSummonerRankChangesQuery : IRequest<List<LoLGameRankChangeDto>>
    {
        /// <summary>
        /// Gets or sets player Id.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the queue restriction. <see cref="LoLQueueFilter.All"/> means both ranked queues.
        /// </summary>
        public LoLQueueFilter Queue { get; set; } = LoLQueueFilter.All;

        /// <summary>
        /// Gets or sets how many of the most recent games to return. Defaults to 50.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets how many days back to look. No restriction when null.
        /// </summary>
        public int? Days { get; set; }
    }
}
