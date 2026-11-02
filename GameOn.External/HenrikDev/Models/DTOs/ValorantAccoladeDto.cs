// <copyright file="ValorantAccoladeDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// An accolade (end-of-game gold star) earned in one match.
    /// </summary>
    public class ValorantAccoladeDto
    {
        /// <summary>
        /// Gets or sets the accolade UUID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accolade type (e.g. kills, clutches, top_frag).
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the value that earned it.
        /// </summary>
        [JsonProperty("value")]
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is the player's record of the act.
        /// </summary>
        [JsonProperty("is_act_record")]
        public bool IsActRecord { get; set; }
    }
}
