// <copyright file="LoLFactOfTheWeekDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using GameOn.Domain;

    /// <summary>
    /// LoLFactOfTheWeekDto class. Highlights the crew member with the best net LP progression this
    /// calendar week (Monday to now, Europe/Paris clock), across every ranked queue they play. Always
    /// the top gainer of the current week, whether or not it happens to be a personal or crew best.
    /// </summary>
    public class LoLFactOfTheWeekDto
    {
        /// <summary>
        /// Gets or sets the player with the best net LP progression this week.
        /// </summary>
        public Player Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets the player's net LP change this week, summed across every ranked queue
        /// (Solo/Duo and Flex) they have a snapshot for both this week and last week. Positive means
        /// net gain, negative means net loss. Promotions/demotions between snapshots are handled on a
        /// continuous inter-tier scale rather than ignored (see LoLRankScaleCalculator).
        /// </summary>
        public int LpChange { get; set; }

        /// <summary>
        /// Gets or sets the number of games the player played this week (all queues except bot/custom/tutorial).
        /// </summary>
        public int GamesThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of games the player won this week.
        /// </summary>
        public int WinsThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the player's win rate this week, as a percentage (0 when no games were played).
        /// </summary>
        public double WinRateThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the longest run of consecutive wins the player put together this week.
        /// </summary>
        public int LongestWinStreakThisWeek { get; set; }
    }
}
