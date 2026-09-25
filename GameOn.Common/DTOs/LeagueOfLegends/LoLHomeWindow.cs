// <copyright file="LoLHomeWindow.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLHomeWindow enum. The time window the "this week" blocks of <c>GET lol/Home</c> cover
    /// (<see cref="LoLHomeStatsDto.WeeklyActivity"/> and <see cref="LoLHomeStatsDto.FactOfTheWeek"/>). Days
    /// are cut on the players' wall clock (Europe/Paris), not on the UTC clock games are stored in.
    /// </summary>
    public enum LoLHomeWindow
    {
        /// <summary>
        /// The current calendar week, Monday 00:00 to now, compared against the previous full Monday to
        /// Sunday week. The default, and the historical behavior of the route: on a Friday, five days are
        /// compared against seven.
        /// </summary>
        CalendarWeek,

        /// <summary>
        /// The last seven days, six days ago 00:00 to now (today included), compared against the seven full
        /// days before that.
        /// </summary>
        Last7Days,
    }
}
