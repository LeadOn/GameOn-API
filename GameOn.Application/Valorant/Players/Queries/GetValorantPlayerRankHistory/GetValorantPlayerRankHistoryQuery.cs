// <copyright file="GetValorantPlayerRankHistoryQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerRankHistory
{
    using System.Collections.Generic;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetValorantPlayerRankHistoryQuery class.
    /// </summary>
    public class GetValorantPlayerRankHistoryQuery : IRequest<IEnumerable<ValorantRankHistory>>
    {
        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of entries to return when <see cref="Granularity"/> is null.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets the optional time bucket granularity (Day, Week, Month).
        /// </summary>
        public LoLRankHistoryGranularity? Granularity { get; set; }

        /// <summary>
        /// Gets or sets how many days back to look when <see cref="Granularity"/> is set.
        /// </summary>
        public int? Days { get; set; }
    }
}