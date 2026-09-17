// <copyright file="LcuParticipantDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    /// <summary>
    /// LcuParticipantDto class. One of the ten slots of a game, as served by the League client.
    /// </summary>
    public class LcuParticipantDto
    {
        /// <summary>
        /// Gets or sets the participant ID (1 to 10).
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the team ID (100 or 200).
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the champion ID. The client never sends the champion name, it is resolved
        /// on import through Community Dragon.
        /// </summary>
        public int ChampionId { get; set; }

        /// <summary>
        /// Gets or sets the first summoner spell ID. Used to spot the jungler (Smite, ID 11).
        /// </summary>
        public int Spell1Id { get; set; }

        /// <summary>
        /// Gets or sets the second summoner spell ID.
        /// </summary>
        public int Spell2Id { get; set; }

        /// <summary>
        /// Gets or sets end of game stats.
        /// </summary>
        public LcuParticipantStatsDto? Stats { get; set; }

        /// <summary>
        /// Gets or sets the client's lane and role guess.
        /// </summary>
        public LcuParticipantTimelineDto? Timeline { get; set; }
    }
}
