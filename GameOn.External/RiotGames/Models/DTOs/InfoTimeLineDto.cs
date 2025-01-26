// <copyright file="InfoTimeLineDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// InfoTimeLineDto class.
    /// </summary>
    public class InfoTimeLineDto
    {
        /// <summary>
        /// Gets or sets end of game result.
        /// </summary>
        [JsonProperty("endOfGameResult")]
        public string EndOfGameResult { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Frame interval.
        /// </summary>
        [JsonProperty("frameInterval")]
        public long FrameInterval { get; set; }

        /// <summary>
        /// Gets or sets Game ID.
        /// </summary>
        [JsonProperty("gameId")]
        public long GameId { get; set; }

        /// <summary>
        /// Gets or sets participants.
        /// </summary>
        [JsonProperty("participants")]
        public List<ParticipantTimeLineDto> Participants { get; set; } = new List<ParticipantTimeLineDto>();

        /// <summary>
        /// Gets or sets frames.
        /// </summary>
        [JsonProperty("frames")]
        public List<FramesTimeLineDto> Frames { get; set; } = new List<FramesTimeLineDto>();
    }
}
