// <copyright file="MatchDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// MatchDto class.
    /// </summary>
    public class MatchDto
    {
        /// <summary>
        /// Gets or sets match metadata.
        /// </summary>
        [JsonProperty("metadata")]
        public MetadataDto Metadata { get; set; } = new MetadataDto();

        /// <summary>
        /// Gets or sets match info.
        /// </summary>
        [JsonProperty("info")]
        public InfoDto Info { get; set; } = new InfoDto();
    }
}
