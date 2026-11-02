// <copyright file="ValorantAgentMasteryListDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Payload of agent-mastery v1 for every agent.
    /// </summary>
    public class ValorantAgentMasteryListDto
    {
        /// <summary>
        /// Gets or sets the mastery of each agent.
        /// </summary>
        [JsonProperty("agents")]
        public List<ValorantAgentMasteryDto> Agents { get; set; } = new List<ValorantAgentMasteryDto>();
    }
}
