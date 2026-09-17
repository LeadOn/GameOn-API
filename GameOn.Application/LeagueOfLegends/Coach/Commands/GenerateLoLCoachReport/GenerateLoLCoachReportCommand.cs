// <copyright file="GenerateLoLCoachReportCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Commands.GenerateLoLCoachReport
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// Generates the AI coach report for one player on one game, or returns the one already stored.
    /// </summary>
    public class GenerateLoLCoachReportCommand : IRequest<LoLCoachReportDto?>
    {
        /// <summary>
        /// Gets or sets the match to analyse.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the GameOn! player to analyse.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an existing report should be thrown away and written again.
        /// Reserved for administrators: every regeneration is a paid call for an answer that already exists.
        /// </summary>
        public bool ForceRegenerate { get; set; }
    }
}
