// <copyright file="ValActivePlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// One entry of <see cref="ValWeeklyActivityDto.ActivePlayers"/>: a tracked account and how much
    /// competitive it played over the window.
    /// </summary>
    public class ValActivePlayerDto
    {
        /// <summary>
        /// Gets or sets the account. Identity fields only.
        /// </summary>
        public PlayerDto Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of competitive games this account played over the window. Always at least 1.
        /// </summary>
        public int Games { get; set; }

        /// <summary>
        /// Gets or sets the number of those games won.
        /// </summary>
        public int Wins { get; set; }
    }
}
