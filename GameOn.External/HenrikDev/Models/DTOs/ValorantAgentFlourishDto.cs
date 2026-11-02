// <copyright file="ValorantAgentFlourishDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Flourish levels of an agent mastery.
    /// </summary>
    public class ValorantAgentFlourishDto
    {
        /// <summary>
        /// Gets or sets the short flourish level.
        /// </summary>
        [JsonProperty("short_level")]
        public int? ShortLevel { get; set; }

        /// <summary>
        /// Gets or sets the long flourish level.
        /// </summary>
        [JsonProperty("long_level")]
        public int? LongLevel { get; set; }
    }
}
