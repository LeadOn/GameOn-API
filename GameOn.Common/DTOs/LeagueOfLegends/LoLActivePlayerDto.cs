// <copyright file="LoLActivePlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLActivePlayerDto class. One entry of <see cref="LoLWeeklyActivityDto.ActivePlayers"/>: a tracked
    /// account and how much ranked it played (Solo/Duo and Flex) over the window.
    /// </summary>
    public class LoLActivePlayerDto
    {
        /// <summary>
        /// Gets or sets the account. Identity fields only: ranks, recent form and performance stats are left
        /// empty here.
        /// </summary>
        public PlayerDto Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of ranked games this account played over the window. Always at least 1:
        /// an account without any game over the window isn't listed.
        /// </summary>
        public int Games { get; set; }

        /// <summary>
        /// Gets or sets the number of those games won. 0 when every game was lost.
        /// </summary>
        public int Wins { get; set; }
    }
}
