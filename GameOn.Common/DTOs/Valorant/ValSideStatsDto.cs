// <copyright file="ValSideStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// A player's figures on one side (attack or defense), summed over the requested games.
    /// </summary>
    public class ValSideStatsDto
    {
        /// <summary>
        /// Gets or sets the rounds played on this side.
        /// </summary>
        public int Rounds { get; set; }

        /// <summary>
        /// Gets or sets the rounds won on this side.
        /// </summary>
        public int RoundsWon { get; set; }

        /// <summary>
        /// Gets or sets the share of rounds won, in percent.
        /// </summary>
        public double RoundWinRatePercent { get; set; }

        /// <summary>
        /// Gets or sets the kills.
        /// </summary>
        public int Kills { get; set; }

        /// <summary>
        /// Gets or sets the deaths.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Gets or sets the kills per death.
        /// </summary>
        public double KdRatio { get; set; }

        /// <summary>
        /// Gets or sets the average combat score per round on this side.
        /// </summary>
        public double Acs { get; set; }
    }
}
