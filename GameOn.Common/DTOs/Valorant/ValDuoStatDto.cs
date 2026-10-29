// <copyright file="ValDuoStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// How often a Valorant account teamed up with another tracked GameOn player, and how well.
    /// </summary>
    public class ValDuoStatDto
    {
        /// <summary>
        /// Gets or sets the teammate. Identity fields only: ranks, recent form and performance stats are left empty.
        /// </summary>
        public PlayerDto Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of games played together (same match, same team), premade or not.
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets how many of <see cref="GamesPlayed"/> were played in the same party (same
        /// <see cref="Domain.ValGameParticipant.PartyId"/>), as opposed to being matched together by chance.
        /// </summary>
        public int PremadeGames { get; set; }

        /// <summary>
        /// Gets or sets the number of games won together.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the win rate of the games played together, as a percentage.
        /// </summary>
        public double WinRate { get; set; }
    }
}
