// <copyright file="GetSummonerRankHistoryQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetSummonerRankHistory
{
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetSummonerRankHistoryQuery class.
    /// </summary>
    public class GetSummonerRankHistoryQuery : IRequest<IEnumerable<LeagueOfLegendsRankHistory>>
    {
        /// <summary>
        /// Gets or sets player Id.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets limit.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets the bucketing granularity. When set, only the last rank snapshot of each
        /// period (day/week/month) over <see cref="Days"/> is returned, instead of every change.
        /// </summary>
        public LoLRankHistoryGranularity? Granularity { get; set; }

        /// <summary>
        /// Gets or sets how many days back to look when <see cref="Granularity"/> is set.
        /// Defaults to a sensible window for the chosen granularity (21 days / 90 days / 1 year).
        /// </summary>
        public int? Days { get; set; }
    }
}
