// <copyright file="ValorantAgentMasteryDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Mastery of one agent (agent-mastery v1).
    /// </summary>
    public class ValorantAgentMasteryDto
    {
        /// <summary>
        /// Gets or sets the agent.
        /// </summary>
        [JsonProperty("agent")]
        public ValorantIdNameDto? Agent { get; set; }

        /// <summary>
        /// Gets or sets the flourish levels.
        /// </summary>
        [JsonProperty("flourish")]
        public ValorantAgentFlourishDto? Flourish { get; set; }

        /// <summary>
        /// Gets or sets the mastery tracks.
        /// </summary>
        [JsonProperty("tracks")]
        public List<ValorantAgentMasteryTrackDto> Tracks { get; set; } = new List<ValorantAgentMasteryTrackDto>();

        /// <summary>
        /// Gets or sets the equipped cosmetics.
        /// </summary>
        [JsonProperty("customization")]
        public ValorantAgentMasteryCustomizationDto? Customization { get; set; }
    }
}
