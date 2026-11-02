// <copyright file="ValBuyTypeStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// A player's rounds by buy type. Standard modes only (competitive, unrated, premier). Eco under 2,000
    /// credits of loadout, force buy from 2,000 to 3,899, full buy from 3,900, pistol rounds counted apart.
    /// </summary>
    public class ValBuyTypeStatsDto
    {
        /// <summary>
        /// Gets or sets the pistol rounds played.
        /// </summary>
        public int PistolRounds { get; set; }

        /// <summary>
        /// Gets or sets the pistol rounds won.
        /// </summary>
        public int PistolRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets the pistol round win rate, in percent.
        /// </summary>
        public double PistolWinRatePercent { get; set; }

        /// <summary>
        /// Gets or sets the eco rounds played.
        /// </summary>
        public int EcoRounds { get; set; }

        /// <summary>
        /// Gets or sets the eco rounds won.
        /// </summary>
        public int EcoRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets the eco round win rate, in percent.
        /// </summary>
        public double EcoWinRatePercent { get; set; }

        /// <summary>
        /// Gets or sets the force-buy rounds played.
        /// </summary>
        public int ForceBuyRounds { get; set; }

        /// <summary>
        /// Gets or sets the force-buy rounds won.
        /// </summary>
        public int ForceBuyRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets the force-buy round win rate, in percent.
        /// </summary>
        public double ForceBuyWinRatePercent { get; set; }

        /// <summary>
        /// Gets or sets the full-buy rounds played.
        /// </summary>
        public int FullBuyRounds { get; set; }

        /// <summary>
        /// Gets or sets the full-buy rounds won.
        /// </summary>
        public int FullBuyRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets the full-buy round win rate, in percent.
        /// </summary>
        public double FullBuyWinRatePercent { get; set; }
    }
}
