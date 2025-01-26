// <copyright file="TimelineDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// TimelineDto class.
    /// </summary>
    public class TimelineDto
    {
        /// <summary>
        /// Gets or sets Metadata Timeline.
        /// </summary>
        [JsonProperty("metadata")]
        public MetadataTimeLineDto Metadata { get; set; } = new MetadataTimeLineDto();

        /// <summary>
        /// Gets or sets Info Timeline.
        /// </summary>
        [JsonProperty("info")]
        public InfoTimeLineDto Info { get; set; } = new InfoTimeLineDto();
    }
}
