// <copyright file="ValDailyActivityDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System;

    /// <summary>
    /// One day of <see cref="ValWeeklyActivityDto.Days"/>: the crew's competitive activity on a single day of
    /// the players' wall clock (Europe/Paris). Same games and same roster as the weekly totals.
    /// </summary>
    public class ValDailyActivityDto
    {
        /// <summary>
        /// Gets or sets the day, on the players' wall clock. A game belongs to the day it started on.
        /// Serialized as <c>yyyy-MM-dd</c>.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Gets or sets the number of competitive games played that day.
        /// </summary>
        public int Games { get; set; }

        /// <summary>
        /// Gets or sets the number of those games won.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the number of those games not won.
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets the cumulated in-game time that day, in minutes, rounded to a tenth. The tenths are
        /// spread across the days (largest remainder first) so that they add up to the weekly total exactly.
        /// </summary>
        public double PlaytimeMinutes { get; set; }

        /// <summary>
        /// Gets or sets the crew's net RR change that day, same rule as
        /// <see cref="ValWeeklyActivityDto.NetRrChangeThisWeek"/>. Null when no ranked match of the MMR
        /// history falls on that day, which is not the same as 0 (gains and losses cancelled out).
        /// </summary>
        public int? NetRrChange { get; set; }
    }
}
