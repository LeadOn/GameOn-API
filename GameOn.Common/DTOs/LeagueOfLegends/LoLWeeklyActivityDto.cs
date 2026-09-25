// <copyright file="LoLWeeklyActivityDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLWeeklyActivityDto class. Squad-wide ranked activity recap (Solo/Duo and Flex), all tracked players
    /// combined, over the window picked by <see cref="LoLHomeWindow"/>: by default the current calendar week
    /// (Monday to now, Europe/Paris clock) against the previous full week, or the last seven days against the
    /// seven before. "This week" and "last week" below stand for these two windows, whichever is in use.
    /// </summary>
    public class LoLWeeklyActivityDto
    {
        /// <summary>
        /// Gets or sets the start of the current window, in UTC: Monday 00:00 Europe/Paris for
        /// <see cref="LoLHomeWindow.CalendarWeek"/>, six days ago 00:00 Europe/Paris for
        /// <see cref="LoLHomeWindow.Last7Days"/>. Inclusive.
        /// </summary>
        public DateTime WindowStart { get; set; }

        /// <summary>
        /// Gets or sets the end of the current window, in UTC: the moment the recap was computed. Inclusive.
        /// </summary>
        public DateTime WindowEnd { get; set; }

        /// <summary>
        /// Gets or sets the start of the previous window, in UTC: seven days of the players' wall clock
        /// before <see cref="WindowStart"/>. Inclusive. The previous window ends where the current one starts
        /// (exclusive), and always covers seven full days, whereas the current one may be partial.
        /// </summary>
        public DateTime PreviousWindowStart { get; set; }

        /// <summary>
        /// Gets or sets the number of games played this week.
        /// </summary>
        public int GamesThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games played last week (the previous window, seven full days).
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
        /// Gets or sets the number of games won last week, on the same games as <see cref="GamesLastWeek"/>.
        /// 0 when nothing was won, or nothing was played.
        /// </summary>
        public int WinsLastWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games lost last week (<see cref="GamesLastWeek"/> minus
        /// <see cref="WinsLastWeek"/>). 0 when nothing was lost, or nothing was played.
        /// </summary>
        public int LossesLastWeek { get; set; }

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

        /// <summary>
        /// Gets or sets this week day by day, oldest first, one entry per day of the players' wall clock
        /// (Europe/Paris) from <see cref="WindowStart"/> to today included: seven entries for
        /// <see cref="LoLHomeWindow.Last7Days"/>, one (Monday) to seven (Sunday) for
        /// <see cref="LoLHomeWindow.CalendarWeek"/>. Days without any game are listed, at 0. Games, wins,
        /// losses and playtime add up to the weekly totals; see <see cref="LoLDailyActivityDto.NetLpChange"/>
        /// for how the LP compare.
        /// </summary>
        public List<LoLDailyActivityDto> Days { get; set; } = new List<LoLDailyActivityDto>();

        /// <summary>
        /// Gets or sets the accounts that played at least one ranked game this week, most games first (ties
        /// broken by wins, then player ID). Same games as <see cref="GamesThisWeek"/>, so their
        /// <see cref="LoLActivePlayerDto.Games"/> add up to it. One entry per account: with smurfs included,
        /// a member's main and smurf are listed separately. Empty when nobody played.
        /// </summary>
        public List<LoLActivePlayerDto> ActivePlayers { get; set; } = new List<LoLActivePlayerDto>();
    }
}
