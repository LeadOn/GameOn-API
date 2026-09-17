// <copyright file="LcuFrameParticipantDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    /// <summary>
    /// LcuFrameParticipantDto class. State of one participant at one timeline frame. The client's
    /// timeline is the legacy one: it carries gold, experience, level and creep score, but none of
    /// the damage and champion stats match-v5 provides.
    /// </summary>
    public class LcuFrameParticipantDto
    {
        /// <summary>
        /// Gets or sets the participant ID.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets current (unspent) gold.
        /// </summary>
        public int CurrentGold { get; set; }

        /// <summary>
        /// Gets or sets total gold earned so far.
        /// </summary>
        public int TotalGold { get; set; }

        /// <summary>
        /// Gets or sets champion level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets minions killed so far.
        /// </summary>
        public int MinionsKilled { get; set; }

        /// <summary>
        /// Gets or sets neutral monsters killed so far.
        /// </summary>
        public int JungleMinionsKilled { get; set; }

        /// <summary>
        /// Gets or sets experience earned so far.
        /// </summary>
        public int Xp { get; set; }

        /// <summary>
        /// Gets or sets the position on the map.
        /// </summary>
        public LcuPositionDto? Position { get; set; }
    }
}
