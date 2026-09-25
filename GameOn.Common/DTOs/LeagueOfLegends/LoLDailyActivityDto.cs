// <copyright file="LoLDailyActivityDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLDailyActivityDto class. One day of <see cref="LoLWeeklyActivityDto.Days"/>: the crew's ranked
    /// activity (Solo/Duo and Flex) on a single day of the players' wall clock (Europe/Paris). Same games,
    /// same roster and same rules as the weekly totals, so that the days add up to them.
    /// </summary>
    public class LoLDailyActivityDto
    {
        /// <summary>
        /// Gets or sets the day, on the players' wall clock (Europe/Paris). A game belongs to the day it
        /// started on. Serialized as <c>yyyy-MM-dd</c>.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Gets or sets the number of ranked games played that day. 0 on a day without any game: such days
        /// are listed all the same, so the list always covers the whole window.
        /// </summary>
        public int Games { get; set; }

        /// <summary>
        /// Gets or sets the number of those games won. 0 when nothing was won, or nothing was played.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the number of those games lost (<see cref="Games"/> minus <see cref="Wins"/>).
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets the cumulated in-game time that day, in minutes, rounded to a tenth. Same duration
        /// rule as <see cref="LoLWeeklyActivityDto.TotalPlaytimeMinutesThisWeek"/>. The tenths are spread
        /// across the days (largest remainder first) so that they add up to the weekly total exactly, which
        /// means a day may read a tenth off its own rounding. 0 on a day without any game.
        /// </summary>
        public double PlaytimeMinutes { get; set; }

        /// <summary>
        /// Gets or sets the crew's net LP change that day, summed across every tracked account and ranked
        /// queue, on the continuous cross-tier scale of <see cref="LoLWeeklyActivityDto.NetLpChangeThisWeek"/>.
        /// For each account and queue, the last rank snapshot of the day is compared against the last
        /// snapshot taken before the day started, however old: snapshots are only written when the rank
        /// moved, so the last one before the day is where the account stood when the day began. Null when
        /// no account has a comparable pair that day (no snapshot during the day, no snapshot before it, or
        /// a tier outside the scale such as unranked), which is not the same as 0: 0 means ranked LP did
        /// move and the gains and losses cancelled out exactly.
        /// </summary>
        /// <remarks>
        /// The days add up to <see cref="LoLWeeklyActivityDto.NetLpChangeThisWeek"/> except in two cases.
        /// First, an account whose last snapshot before the window is older than the previous window: the
        /// weekly total has no previous-window snapshot to compare that account against and leaves it out,
        /// while its first day here is compared against that older snapshot and counts. Second, a tier
        /// outside the scale in the middle of the window (placements after a season reset): the weekly total
        /// only compares both ends of the window, the days compare every step, and a step with an unplaceable
        /// end is dropped.
        /// </remarks>
        public int? NetLpChange { get; set; }
    }
}
