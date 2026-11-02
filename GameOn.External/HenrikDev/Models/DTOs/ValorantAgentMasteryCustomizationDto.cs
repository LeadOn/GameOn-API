// <copyright file="ValorantAgentMasteryCustomizationDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Cosmetics equipped on an agent mastery.
    /// </summary>
    public class ValorantAgentMasteryCustomizationDto
    {
        /// <summary>
        /// Gets or sets the hype cosmetic.
        /// </summary>
        [JsonProperty("hype")]
        public ValorantIdNameDto? Hype { get; set; }

        /// <summary>
        /// Gets or sets the background cosmetic.
        /// </summary>
        [JsonProperty("background")]
        public ValorantIdNameDto? Background { get; set; }

        /// <summary>
        /// Gets or sets the clip cosmetic.
        /// </summary>
        [JsonProperty("clip")]
        public ValorantIdNameDto? Clip { get; set; }

        /// <summary>
        /// Gets or sets the border cosmetic.
        /// </summary>
        [JsonProperty("border")]
        public ValorantIdNameDto? Border { get; set; }
    }
}
