// <copyright file="LcuParticipantTimelineDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    /// <summary>
    /// LcuParticipantTimelineDto class. The client's guess at where a participant played.
    /// Unreliable in custom games (it routinely reports two junglers per team), hence the
    /// heuristics applied on import rather than a straight copy.
    /// </summary>
    public class LcuParticipantTimelineDto
    {
        /// <summary>
        /// Gets or sets the lane (TOP, JUNGLE, MIDDLE, BOTTOM, NONE).
        /// </summary>
        public string? Lane { get; set; }

        /// <summary>
        /// Gets or sets the role (SOLO, CARRY, SUPPORT, NONE).
        /// </summary>
        public string? Role { get; set; }
    }
}
