// <copyright file="LoLSummonerPerformanceStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLSummonerPerformanceStatsDto class. Performance recap for a single summoner over a rolling
    /// time window (see <see cref="LoLStatsPeriod"/>), all queues combined.
    /// </summary>
    public class LoLSummonerPerformanceStatsDto
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
        /// Gets or sets the number of games lost in the period.
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets the win rate percentage. 0 when <see cref="GamesPlayed"/> is 0.
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
        /// Gets or sets the average KDA ratio across games (mean of each game's
        /// <see cref="Domain.LoLGameParticipantStat.Kda"/>).
        /// </summary>
        public double AverageKda { get; set; }

        /// <summary>
        /// Gets or sets the average creep score per minute across games.
        /// </summary>
        public double AverageCsPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the average damage dealt to champions per minute across games.
        /// </summary>
        public double AverageDamagePerMinute { get; set; }

        /// <summary>
        /// Gets or sets the average vision score per game (Riot's <c>visionScore</c>, not a per-minute rate).
        /// </summary>
        public double AverageVisionScore { get; set; }
    }
}
