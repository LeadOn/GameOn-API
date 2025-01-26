// <copyright file="ParticipantFramesDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// ParticipantFramesDto class.
    /// </summary>
    public class ParticipantFramesDto
    {
        /// <summary>
        /// Gets or sets 1.
        /// </summary>
        [JsonProperty("1")]
        public ParticipantFrameDto Participant1 { get; set; } = new ParticipantFrameDto();

        /// <summary>
        /// Gets or sets 2.
        /// </summary>
        [JsonProperty("2")]
        public ParticipantFrameDto Participant2 { get; set; } = new ParticipantFrameDto();

        /// <summary>
        /// Gets or sets 3.
        /// </summary>
        [JsonProperty("3")]
        public ParticipantFrameDto Participant3 { get; set; } = new ParticipantFrameDto();

        /// <summary>
        /// Gets or sets 4.
        /// </summary>
        [JsonProperty("4")]
        public ParticipantFrameDto Participant4 { get; set; } = new ParticipantFrameDto();

        /// <summary>
        /// Gets or sets 5.
        /// </summary>
        [JsonProperty("5")]
        public ParticipantFrameDto Participant5 { get; set; } = new ParticipantFrameDto();

        /// <summary>
        /// Gets or sets 6.
        /// </summary>
        [JsonProperty("6")]
        public ParticipantFrameDto Participant6 { get; set; } = new ParticipantFrameDto();

        /// <summary>
        /// Gets or sets 7.
        /// </summary>
        [JsonProperty("7")]
        public ParticipantFrameDto Participant7 { get; set; } = new ParticipantFrameDto();

        /// <summary>
        /// Gets or sets 8.
        /// </summary>
        [JsonProperty("8")]
        public ParticipantFrameDto Participant8 { get; set; } = new ParticipantFrameDto();

        /// <summary>
        /// Gets or sets 9.
        /// </summary>
        [JsonProperty("9")]
        public ParticipantFrameDto Participant9 { get; set; } = new ParticipantFrameDto();

        /// <summary>
        /// Gets or sets 10.
        /// </summary>
        [JsonProperty("10")]
        public ParticipantFrameDto Participant10 { get; set; } = new ParticipantFrameDto();
    }
}
