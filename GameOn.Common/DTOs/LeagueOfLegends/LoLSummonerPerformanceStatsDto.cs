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

        /// <summary>
        /// Gets or sets the breakdown of every champion played in the period, most played first
        /// (ties broken by win rate, then champion name for a stable order).
        /// </summary>
        public List<LoLChampionStatDto> ChampionStats { get; set; } = new List<LoLChampionStatDto>();

        /// <summary>
        /// Gets or sets the breakdown of roles played in the period, most played first (ties broken by
        /// win rate, then team position for a stable order). Games whose role Riot could not resolve
        /// (<c>teamPosition</c> empty — imports older than the Phase 1 Riot-data capture, or game modes
        /// without a lane) are excluded from both the numerator and the denominator, so <see cref="LoLRoleStatDto.PlayRate"/>
        /// reads against games with a resolved role only, not every game in the period.
        /// </summary>
        public List<LoLRoleStatDto> RoleStats { get; set; } = new List<LoLRoleStatDto>();

        /// <summary>
        /// Gets or sets the breakdown of games played alongside other tracked GameOn players in the period
        /// (same match, same team), most played first (ties broken by win rate, then teammate ID for a
        /// stable order). Teammates who aren't linked to a GameOn player are not counted.
        /// </summary>
        public List<LoLDuoStatDto> DuoStats { get; set; } = new List<LoLDuoStatDto>();
    }
}
