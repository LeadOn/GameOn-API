// <copyright file="LcuParticipantIdentityDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    /// <summary>
    /// LcuParticipantIdentityDto class. Links a participant slot to the player who filled it.
    /// </summary>
    public class LcuParticipantIdentityDto
    {
        /// <summary>
        /// Gets or sets the participant ID (1 to 10), matching <see cref="LcuParticipantDto.ParticipantId"/>.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the player behind that participant.
        /// </summary>
        public LcuPlayerDto? Player { get; set; }
    }
}
