// <copyright file="LoLChampionStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLChampionStatDto class. How much a champion was played by the tracked crew over a given period,
    /// and how well.
    /// </summary>
    public class LoLChampionStatDto
    {
        /// <summary>
        /// Gets or sets the champion's name (Riot's internal name, ex: "MonkeyKing" for Wukong).
        /// </summary>
        public string ChampionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of games played on this champion, across every tracked player.
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets the number of games won on this champion.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the win rate on this champion, as a percentage.
        /// </summary>
        public double WinRate { get; set; }
    }
}
