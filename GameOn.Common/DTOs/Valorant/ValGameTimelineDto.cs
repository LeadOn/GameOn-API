// <copyright file="ValGameTimelineDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System.Collections.Generic;
    using GameOn.Domain;

    /// <summary>
    /// Round-by-round breakdown and chronological kill events for a Valorant match.
    /// </summary>
    public class ValGameTimelineDto
    {
        /// <summary>
        /// Gets or sets the Valorant match UUID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rounds of the match, ordered by <see cref="ValGameRound.RoundNumber"/>.
        /// </summary>
        public List<ValGameRound> Rounds { get; set; } = new List<ValGameRound>();

        /// <summary>
        /// Gets or sets the kill events of the match, ordered by round and timestamp.
        /// </summary>
        public List<ValGameKill> Kills { get; set; } = new List<ValGameKill>();

        /// <summary>
        /// Gets or sets each player's buy, combat score, hits and damage per round, ordered by round, team
        /// and PUUID. Empty for matches imported before round stats were stored, until they are re-imported.
        /// </summary>
        public List<ValGameRoundPlayerStat> RoundPlayerStats { get; set; } = new List<ValGameRoundPlayerStat>();
    }
}
