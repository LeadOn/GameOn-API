// <copyright file="ValTradeStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// A player's trades: kills avenging a teammate, and deaths a teammate avenged, within 5 seconds.
    /// </summary>
    public class ValTradeStatsDto
    {
        /// <summary>
        /// Gets or sets the kills that avenged a teammate.
        /// </summary>
        public int TradeKills { get; set; }

        /// <summary>
        /// Gets or sets the deaths a teammate avenged.
        /// </summary>
        public int TradedDeaths { get; set; }

        /// <summary>
        /// Gets or sets the deaths in the games with trade data.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Gets or sets the share of deaths that were traded, in percent.
        /// </summary>
        public double TradedDeathPercent { get; set; }
    }
}
