// <copyright file="GetSummonerRankHistoryQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetSummonerRankHistory
{
    using GameOn.Common.DTOs;
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
    }
}
