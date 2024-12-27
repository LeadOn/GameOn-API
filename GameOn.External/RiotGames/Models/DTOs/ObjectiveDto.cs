// <copyright file="ObjectiveDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// ObjectiveDto class.
    /// </summary>
    public class ObjectiveDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether objective was first or not.
        /// </summary>
        [JsonProperty("first")]
        public bool First { get; set; }

        /// <summary>
        /// Gets or sets kills associated with objective.
        /// </summary>
        [JsonProperty("kills")]
        public int Kills { get; set; }
    }
}
