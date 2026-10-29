// <copyright file="ValorantPlayerPerformanceStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System.Collections.Generic;

    /// <summary>
    /// Performance recap of a single Valorant account over a rolling time window, on the games left after the
    /// query filters (period, queues, agent). Remakes and games without a real round structure (customs,
    /// deathmatch, team deathmatch) are never counted.
    /// </summary>
    /// <remarks>
    /// Every rate is computed on the totals of the period rather than averaged game by game: ACS is the score
    /// of every round played divided by the number of rounds, not the mean of each game's ACS. A 13-11 game
    /// therefore weighs more than a 9-round Swiftplay, as it should. Games whose derived stats
    /// (<see cref="Domain.ValGameParticipantStat"/>) are missing still count towards the games, wins, playtime
    /// and KDA, but are left out of every per-round rate.
    /// </remarks>
    public class ValorantPlayerPerformanceStatsDto
    {
        /// <summary>
        /// Gets or sets the number of games played in the period.
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets the number of games won in the period.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the number of games lost in the period. Draws are not losses.
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets the number of games drawn in the period (no winning team). Only unrated-like queues
        /// allow a draw.
        /// </summary>
        public int Draws { get; set; }

        /// <summary>
        /// Gets or sets the win rate percentage, draws included in the denominator.
        /// </summary>
        public double WinRatePercent { get; set; }

        /// <summary>
        /// Gets or sets the total time played in the period, in seconds.
        /// </summary>
        public long TotalPlaytimeSeconds { get; set; }

        /// <summary>
        /// Gets or sets the average game duration, in seconds.
        /// </summary>
        public double AverageGameDurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds played in the period, over the games that have derived stats.
        /// </summary>
        public int RoundsPlayed { get; set; }

        /// <summary>
        /// Gets or sets the Average Combat Score: total score divided by total rounds played.
        /// </summary>
        public double AverageAcs { get; set; }

        /// <summary>
        /// Gets or sets the Average Damage per Round: total damage dealt divided by total rounds played.
        /// </summary>
        public double AverageAdr { get; set; }

        /// <summary>
        /// Gets or sets the KDA ratio over the period: <c>(kills + assists) / max(deaths, 1)</c>, on totals.
        /// </summary>
        public double Kda { get; set; }

        /// <summary>
        /// Gets or sets the K/D ratio over the period: <c>kills / max(deaths, 1)</c>, on totals.
        /// </summary>
        public double KdRatio { get; set; }

        /// <summary>
        /// Gets or sets the headshot percentage over every shot that hit (head, body and legs).
        /// </summary>
        public double HeadshotPercent { get; set; }

        /// <summary>
        /// Gets or sets the KAST percentage, weighted by the rounds of each game.
        /// </summary>
        public double KastPercent { get; set; }

        /// <summary>
        /// Gets or sets the mean GameOn! rating (<see cref="Domain.ValGameParticipantStat.Rating"/>, out of 10)
        /// of the games that have derived stats. A per-game score, so averaged game by game.
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds this account opened with the first kill.
        /// </summary>
        public int FirstKills { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds this account was the first to die.
        /// </summary>
        public int FirstDeaths { get; set; }

        /// <summary>
        /// Gets or sets the breakdown of every agent played in the period, most played first (ties broken by
        /// win rate, then agent name for a stable order).
        /// </summary>
        public List<ValAgentStatDto> AgentStats { get; set; } = new List<ValAgentStatDto>();

        /// <summary>
        /// Gets or sets the breakdown of every map played in the period, most played first (ties broken by win
        /// rate, then map name for a stable order).
        /// </summary>
        public List<ValMapStatDto> MapStats { get; set; } = new List<ValMapStatDto>();

        /// <summary>
        /// Gets or sets the breakdown of games played alongside other tracked GameOn players (same match, same
        /// team), most played first (ties broken by win rate, then teammate ID for a stable order). Per
        /// account: a member's main and smurf are two partners. Teammates not linked to a GameOn player are
        /// not counted.
        /// </summary>
        public List<ValDuoStatDto> DuoStats { get; set; } = new List<ValDuoStatDto>();
    }
}
