// <copyright file="LoLCoachAnalysisDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Structured analysis produced by the AI coach. Mirrors the response schema the model is constrained to,
    /// so the payload can be round-tripped without any parsing of free-form prose.
    /// </summary>
    public class LoLCoachAnalysisDto
    {
        /// <summary>
        /// Gets or sets the overall verdict, in a few sentences.
        /// </summary>
        [JsonPropertyName("synthese")]
        public string Synthese { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets what the player did well. Legitimately empty when the game contained nothing positive.
        /// </summary>
        [JsonPropertyName("pointsForts")]
        public List<string> PointsForts { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the improvement areas.
        /// </summary>
        [JsonPropertyName("axesProgression")]
        public List<LoLCoachImprovementAxisDto> AxesProgression { get; set; } = new List<LoLCoachImprovementAxisDto>();

        /// <summary>
        /// Gets or sets the rating out of 10 given to the performance.
        /// </summary>
        [JsonPropertyName("noteSur10")]
        public double NoteSur10 { get; set; }
    }
}
