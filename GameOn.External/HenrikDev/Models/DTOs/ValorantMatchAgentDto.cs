// <copyright file="ValorantMatchAgentDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Agent descriptor inside a HenrikDev v4 match player entry.
    /// </summary>
    public class ValorantMatchAgentDto
    {
        /// <summary>
        /// Gets or sets the agent UUID.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the agent display name (e.g. "Jett", "Raze", "Omen").
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
