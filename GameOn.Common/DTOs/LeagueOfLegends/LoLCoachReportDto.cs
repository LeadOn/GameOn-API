// <copyright file="LoLCoachReportDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using System;

    /// <summary>
    /// An AI coach report as exposed by the API.
    /// </summary>
    public class LoLCoachReportDto
    {
        /// <summary>
        /// Gets or sets the analysed match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the analysed player.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the analysis itself. Never null: a report only exists once it has been generated.
        /// </summary>
        public LoLCoachAnalysisDto Analysis { get; set; } = new LoLCoachAnalysisDto();

        /// <summary>
        /// Gets or sets the model that produced the analysis.
        /// </summary>
        public string? ModelName { get; set; }

        /// <summary>
        /// Gets or sets the date the analysis was generated.
        /// </summary>
        public DateTime GeneratedOn { get; set; }
    }
}
