// <copyright file="GetLastGamesPlayedQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

using GameOn.Common.DTOs.Common;

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetLastGamesPlayed
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetLastGamesPlayedQuery class.
    /// </summary>
    public class GetLastGamesPlayedQuery : IRequest<ListResultDto<LoLGame>?>
    {
        /// <summary>
        /// Gets or sets player Id.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets page.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets number of results.
        /// </summary>
        public int NumberOfResults { get; set; }
    }
}
