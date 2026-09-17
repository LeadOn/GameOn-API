// <copyright file="GetLoLCoachReportQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Queries.GetLoLCoachReport
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// Gets the AI coach report written for one player on one game.
    /// </summary>
    public class GetLoLCoachReportQuery : IRequest<LoLCoachReportDto?>
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
