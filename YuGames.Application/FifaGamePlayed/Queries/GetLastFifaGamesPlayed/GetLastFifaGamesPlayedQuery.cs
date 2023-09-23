// <copyright file="GetLastFifaGamesPlayedQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Queries.GetLastFifaGamesPlayed
{
    using MediatR;
    using YuGames.Common.DTOs;

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
