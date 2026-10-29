// <copyright file="ValGlobalStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System.Collections.Generic;

    /// <summary>
    /// Crew-wide Valorant fun stats recap. Every award is null when nobody holds a meaningful record over the
    /// filters (an award at zero is never handed out to an arbitrary account).
    /// </summary>
    public class ValGlobalStatsDto
    {
        /// <summary>
        /// Gets or sets the number of distinct games analyzed.
        /// </summary>
        public int TotalGamesAnalyzed { get; set; }

        /// <summary>
        /// Gets or sets the number of distinct accounts with at least one game analyzed.
        /// </summary>
        public int TotalPlayersTracked { get; set; }

        /// <summary>
        /// Gets or sets the Sharpshooter award: best headshot percentage over the period, among accounts with
        /// enough games for the ratio to mean something.
        /// </summary>
        public ValFunStatDto? Sharpshooter { get; set; }

        /// <summary>
        /// Gets or sets the Bodyshot Enjoyer award: worst headshot percentage over the period, same pool as
        /// <see cref="Sharpshooter"/>.
        /// </summary>
        public ValFunStatDto? BodyshotEnjoyer { get; set; }

        /// <summary>
        /// Gets or sets the Highest Combat Score award: best ACS in a single game.
        /// </summary>
        public ValFunStatDto? HighestCombatScore { get; set; }

        /// <summary>
        /// Gets or sets the Entry King award: most first kills in a single game.
        /// </summary>
        public ValFunStatDto? EntryKing { get; set; }

        /// <summary>
        /// Gets or sets the First Blood Victim award: most rounds opened by dying first in a single game.
        /// </summary>
        public ValFunStatDto? FirstBloodVictim { get; set; }

        /// <summary>
        /// Gets or sets the Biggest Inter award: most deaths in a single game.
        /// </summary>
        public ValFunStatDto? BiggestInter { get; set; }

        /// <summary>
        /// Gets or sets the Spike Carrier award: most spike plants and defuses combined in a single game.
        /// </summary>
        public ValFunStatDto? SpikeCarrier { get; set; }

        /// <summary>
        /// Gets or sets the Knife Assassin award: most melee kills over the whole period.
        /// </summary>
        public ValFunStatDto? KnifeAssassin { get; set; }

        /// <summary>
        /// Gets or sets the Night Owl award: most games started between midnight and 6 AM (Europe/Paris).
        /// </summary>
        public ValFunStatDto? NightOwl { get; set; }

        /// <summary>
        /// Gets or sets the Longest Win Streak award: most consecutive wins.
        /// </summary>
        public ValFunStatDto? LongestWinStreak { get; set; }

        /// <summary>
        /// Gets or sets the Longest Loss Streak award: most consecutive losses (a draw breaks the streak).
        /// </summary>
        public ValFunStatDto? LongestLossStreak { get; set; }

        /// <summary>
        /// Gets or sets the crew's 5 most played agents, most played first (ties broken by win rate, then agent
        /// name). Empty when no game matches the filters.
        /// </summary>
        public List<ValCrewAgentStatDto> TopAgents { get; set; } = new List<ValCrewAgentStatDto>();

        /// <summary>
        /// Gets or sets the crew's 5 most played maps, most played first (ties broken by win rate, then map
        /// name). A game counts once per crew team (see <see cref="ValMapStatDto.GamesPlayed"/>). Empty when no
        /// game matches the filters.
        /// </summary>
        public List<ValMapStatDto> TopMaps { get; set; } = new List<ValMapStatDto>();
    }
}
