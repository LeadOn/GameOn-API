// <copyright file="LoLCoachImprovementAxisDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// One improvement area raised by the AI coach.
    /// </summary>
    public class LoLCoachImprovementAxisDto
    {
        /// <summary>
        /// Gets or sets the short headline.
        /// </summary>
        [JsonPropertyName("titre")]
        public string Titre { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the explanation, backed by numbers from the game.
        /// </summary>
        [JsonPropertyName("explication")]
        public string Explication { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the concrete action the player can apply in their next game.
        /// </summary>
        [JsonPropertyName("actionConcrete")]
        public string ActionConcrete { get; set; } = string.Empty;
    }
}
