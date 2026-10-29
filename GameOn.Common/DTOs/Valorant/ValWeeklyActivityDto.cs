// <copyright file="ValWeeklyActivityDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System;
    using System.Collections.Generic;
    using GameOn.Common.DTOs.LeagueOfLegends;

    /// <summary>
    /// Crew-wide competitive Valorant activity, over the window picked by <see cref="LoLHomeWindow"/>: the
    /// current calendar week by default, or the last seven days, each against the seven days before. "This
    /// week" and "last week" below stand for these two windows, whichever is in use.
    /// </summary>
    public class ValWeeklyActivityDto
    {
        /// <summary>
        /// Gets or sets the start of the current window, in UTC. Inclusive.
        /// </summary>
        public DateTime WindowStart { get; set; }

        /// <summary>
        /// Gets or sets the end of the current window, in UTC: the moment the recap was computed. Inclusive.
        /// </summary>
        public DateTime WindowEnd { get; set; }

        /// <summary>
        /// Gets or sets the start of the previous window, in UTC: seven days of the players' wall clock before
        /// <see cref="WindowStart"/>. Inclusive. It ends where the current one starts (exclusive).
        /// </summary>
        public DateTime PreviousWindowStart { get; set; }

        /// <summary>
        /// Gets or sets the number of competitive games played this week, one per tracked participation.
        /// </summary>
        public int GamesThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of competitive games played last week.
        /// </summary>
        public int GamesLastWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games won this week.
        /// </summary>
        public int WinsThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games not won this week (<see cref="GamesThisWeek"/> minus
        /// <see cref="WinsThisWeek"/>; competitive has no draws).
        /// </summary>
        public int LossesThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games won last week.
        /// </summary>
        public int WinsLastWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games not won last week.
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
        /// Gets or sets the crew's net RR change this week: the sum of the RR won or lost on every ranked match
        /// recorded in the account's MMR history (<see cref="Domain.ValorantRankHistory.LastChange"/>), matches
        /// started this week only. Promotions and demotions are already folded into Riot's per-match change.
        /// The days add up to it exactly.
        /// </summary>
        public int NetRrChangeThisWeek { get; set; }

        /// <summary>
        /// Gets or sets this week day by day, oldest first, one entry per day of the players' wall clock
        /// (Europe/Paris) from <see cref="WindowStart"/> to today included. Days without any game are listed.
        /// </summary>
        public List<ValDailyActivityDto> Days { get; set; } = new List<ValDailyActivityDto>();

        /// <summary>
        /// Gets or sets the accounts that played at least one competitive game this week, most games first
        /// (ties broken by wins, then player ID). Their games add up to <see cref="GamesThisWeek"/>.
        /// </summary>
        public List<ValActivePlayerDto> ActivePlayers { get; set; } = new List<ValActivePlayerDto>();
    }
}
