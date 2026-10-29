// <copyright file="ValorantMatchCustomizationDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Player card and title customization inside a HenrikDev v4 match player entry.
    /// </summary>
    public class ValorantMatchCustomizationDto
    {
        /// <summary>
        /// Gets or sets the equipped player card UUID.
        /// </summary>
        [JsonProperty("card")]
        public string? Card { get; set; }

        /// <summary>
        /// Gets or sets the equipped player title UUID.
        /// </summary>
        [JsonProperty("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the preferred level border UUID.
        /// </summary>
        [JsonProperty("preferred_level_border")]
        public string? PreferredLevelBorder { get; set; }
    }
}
