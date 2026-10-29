// <copyright file="ValMapStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// How often a Valorant map was played over a period, and how well.
    /// </summary>
    public class ValMapStatDto
    {
        /// <summary>
        /// Gets or sets the map UUID.
        /// </summary>
        public string? MapId { get; set; }

        /// <summary>
        /// Gets or sets the map display name (e.g. "Ascent").
        /// </summary>
        public string MapName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the horizontal map banner URL on <c>media.valorant-api.com</c>. Null when the map UUID is unknown.
        /// </summary>
        public string? MapListViewIconUrl { get; set; }

        /// <summary>
        /// Gets or sets the number of games played on this map. On a crew-wide breakdown, a game counts once
        /// per crew team that played it: a five-stack on Ascent is one game on Ascent, not five.
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets the number of those games won.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the win rate on this map, as a percentage.
        /// </summary>
        public double WinRate { get; set; }

        /// <summary>
        /// Gets or sets the Average Combat Score on this map, over every tracked participation. 0 when no game
        /// has derived stats.
        /// </summary>
        public double Acs { get; set; }
    }
}
