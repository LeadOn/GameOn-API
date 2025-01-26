// <copyright file="ParticipantTimeLineDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// ParticipantTimeLineDto class.
    /// </summary>
    public class ParticipantTimeLineDto
    {
        /// <summary>
        /// Gets or sets participant ID.
        /// </summary>
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets PUUID.
        /// </summary>
        [JsonProperty("puuid")]
        public string PUUID { get; set; } = string.Empty;
    }
}
