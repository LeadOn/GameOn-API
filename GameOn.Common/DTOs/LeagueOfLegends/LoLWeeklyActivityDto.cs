// <copyright file="LoLWeeklyActivityDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLWeeklyActivityDto class. Squad-wide activity recap for the current calendar week (Monday to
    /// now, Europe/Paris clock), all tracked players and games combined.
    /// </summary>
    public class LoLWeeklyActivityDto
    {
        /// <summary>
        /// Gets or sets the number of games played this week (Monday 00:00 to now).
        /// </summary>
        public int GamesThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games played last week (the previous full Monday-to-Sunday week).
        /// </summary>
        public int GamesLastWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games won this week.
        /// </summary>
        public int WinsThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games lost this week.
        /// </summary>
        public int LossesThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the win rate this week, as a percentage (0 when no games were played).
        /// </summary>
        public double WinRateThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the cumulated in-game time this week, in minutes (sum of every tracked participation).
        /// </summary>
        public double TotalPlaytimeMinutesThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the average game duration this week, in minutes (0 when no games were played).
        /// </summary>
        public double AverageGameDurationMinutesThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the squad's cumulated net LP change this week (sum across every tracked player and
        /// ranked queue). Positive means net gain, negative means net loss. Only players with a rank
        /// snapshot both this week and last week on a given queue contribute; a promotion or demotion
        /// between the two snapshots is handled on a continuous inter-tier scale rather than ignored.
        /// </summary>
        public int NetLpChangeThisWeek { get; set; }
    }
}
