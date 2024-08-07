// <copyright file="SearchFifaGamesPlayedQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Queries.SearchFifaGamesPlayed
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// SearchFifaGamesPlayedQuery class.
    /// </summary>
    public class SearchFifaGamesPlayedQuery : IRequest<List<FifaGamePlayedDto>>
    {
        /// <summary>
        /// Gets or sets Limit.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets Platform ID.
        /// </summary>
        public int? PlatformId { get; set; }

        /// <summary>
        /// Gets or sets Start Date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets End Date.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
