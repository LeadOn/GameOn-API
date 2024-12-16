// <copyright file="GetUserNextPlannedMatchsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Queries.GetUserNextPlannedMatchs
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetUserNextPlannedMatchsQuery class.
    /// </summary>
    public class GetUserNextPlannedMatchsQuery : IRequest<IEnumerable<FifaGamePlayedDto>>
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets Limit.
        /// </summary>
        public int Limit { get; set; } = 50;
    }
}
