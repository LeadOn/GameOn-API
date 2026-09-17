// <copyright file="LcuFrameDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    using System.Collections.Generic;

    /// <summary>
    /// LcuFrameDto class. One timeline frame (one per minute of game), as served by the League client.
    /// </summary>
    public class LcuFrameDto
    {
        /// <summary>
        /// Gets or sets the frame timestamp, in milliseconds since the start of the game.
        /// </summary>
        public int Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the state of each participant at that frame, keyed by participant ID as a string.
        /// </summary>
        public Dictionary<string, LcuFrameParticipantDto>? ParticipantFrames { get; set; }

        /// <summary>
        /// Gets or sets the events that happened during that frame.
        /// </summary>
        public List<LcuEventDto>? Events { get; set; }
    }
}
