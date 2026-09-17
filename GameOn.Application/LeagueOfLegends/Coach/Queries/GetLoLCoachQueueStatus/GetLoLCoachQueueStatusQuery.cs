// <copyright file="GetLoLCoachQueueStatusQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Queries.GetLoLCoachQueueStatus
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// Tells where a requested analysis stands in the line, without asking for anything to be written.
    /// </summary>
    public class GetLoLCoachQueueStatusQuery : IRequest<LoLCoachQueueStatusDto?>
    {
        /// <summary>
        /// Gets or sets the match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the GameOn! player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
