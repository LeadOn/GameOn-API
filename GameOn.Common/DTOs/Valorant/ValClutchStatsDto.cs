// <copyright file="ValClutchStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// A player's clutches: rounds where they were left alone against at least one enemy.
    /// </summary>
    public class ValClutchStatsDto
    {
        /// <summary>
        /// Gets or sets the clutch situations.
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// Gets or sets the clutches won.
        /// </summary>
        public int Won { get; set; }

        /// <summary>
        /// Gets or sets the share of clutches won, in percent.
        /// </summary>
        public double WinRatePercent { get; set; }

        /// <summary>
        /// Gets or sets the 1v1 clutches won.
        /// </summary>
        public int Won1v1 { get; set; }

        /// <summary>
        /// Gets or sets the 1v2 clutches won.
        /// </summary>
        public int Won1v2 { get; set; }

        /// <summary>
        /// Gets or sets the 1v3 clutches won.
        /// </summary>
        public int Won1v3 { get; set; }

        /// <summary>
        /// Gets or sets the 1v4 clutches won.
        /// </summary>
        public int Won1v4 { get; set; }

        /// <summary>
        /// Gets or sets the 1v5 clutches won.
        /// </summary>
        public int Won1v5 { get; set; }
    }
}
