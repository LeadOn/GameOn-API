// <copyright file="ValFactOfTheWeekDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// Highlights the account with the best net RR gain over the window of <see cref="ValWeeklyActivityDto"/>.
    /// Always the top gainer of the window, whether or not it is a record.
    /// </summary>
    public class ValFactOfTheWeekDto
    {
        /// <summary>
        /// Gets or sets the account. Identity fields only.
        /// </summary>
        public PlayerDto Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets the account's net RR change over the window, same rule as
        /// <see cref="ValWeeklyActivityDto.NetRrChangeThisWeek"/>. Negative when the whole crew lost RR.
        /// </summary>
        public int RrChange { get; set; }

        /// <summary>
        /// Gets or sets the number of competitive games the account played this week.
        /// </summary>
        public int GamesThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of those games won.
        /// </summary>
        public int WinsThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the win rate this week, as a percentage (0 when no games were played).
        /// </summary>
        public double WinRateThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the longest run of consecutive wins the account put together this week.
        /// </summary>
        public int LongestWinStreakThisWeek { get; set; }
    }
}
