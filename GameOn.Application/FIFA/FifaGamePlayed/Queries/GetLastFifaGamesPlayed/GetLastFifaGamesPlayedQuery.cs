// <copyright file="GetLastFifaGamesPlayedQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaGamePlayed.Queries.GetLastFifaGamesPlayed
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetLastFifaGamesPlayedQuery class.
    /// </summary>
    public class GetLastFifaGamesPlayedQuery : IRequest<List<FifaGamePlayedDto>>
    {
        /// <summary>
        /// Gets or sets Limit.
        /// </summary>
        public int Limit { get; set; }
    }
}
