// <copyright file="ValorantPremierCustomizationDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Premier team icon and colors.
    /// </summary>
    public class ValorantPremierCustomizationDto
    {
        /// <summary>
        /// Gets or sets the icon UUID.
        /// </summary>
        [JsonProperty("icon")]
        public string? Icon { get; set; }

        /// <summary>
        /// Gets or sets the icon image URL.
        /// </summary>
        [JsonProperty("image")]
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the primary color.
        /// </summary>
        [JsonProperty("primary")]
        public string? Primary { get; set; }

        /// <summary>
        /// Gets or sets the secondary color.
        /// </summary>
        [JsonProperty("secondary")]
        public string? Secondary { get; set; }

        /// <summary>
        /// Gets or sets the tertiary color.
        /// </summary>
        [JsonProperty("tertiary")]
        public string? Tertiary { get; set; }
    }
}
