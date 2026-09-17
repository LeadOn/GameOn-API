// <copyright file="EnqueueLoLCoachReportCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Commands.EnqueueLoLCoachReport
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// Asks for one player's analysis of one game to be written, and returns where it stands in the line.
    /// </summary>
    public class EnqueueLoLCoachReportCommand : IRequest<LoLCoachQueueStatusDto?>
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
