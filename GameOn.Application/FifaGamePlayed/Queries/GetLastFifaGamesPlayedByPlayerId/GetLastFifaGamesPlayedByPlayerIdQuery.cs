// <copyright file="GetLastFifaGamesPlayedByPlayerIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Queries.GetLastFifaGamesPlayedByPlayerId
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetLastFifaGamesPlayedByPlayerIdQuery class.
    /// </summary>
    public class GetLastFifaGamesPlayedByPlayerIdQuery : IRequest<List<FifaGamePlayedDto>>
    {
        /// <summary>
        /// Gets or sets Limit.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
