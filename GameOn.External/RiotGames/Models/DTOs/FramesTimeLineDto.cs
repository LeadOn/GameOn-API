// <copyright file="FramesTimeLineDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// FramesTimeLineDto class.
    /// </summary>
    public class FramesTimeLineDto
    {
        /// <summary>
        /// Gets or sets timestamp.
        /// </summary>
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        /// <summary>
        /// Gets or sets participant frames.
        /// </summary>
        [JsonProperty("participantFrames")]
        public ParticipantFramesDto ParticipantFrames { get; set; } = new ParticipantFramesDto();

        /// <summary>
        /// Gets or sets events.
        /// </summary>
        [JsonProperty("events")]
        public List<EventDto> Events { get; set; } = new List<EventDto>();
    }
}
