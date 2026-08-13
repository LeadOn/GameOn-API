// <copyright file="LoLRoleStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLRoleStatDto class. How often a role was played and how well, over a summoner's performance window.
    /// </summary>
    public class LoLRoleStatDto
    {
        /// <summary>
        /// Gets or sets Riot's team position for this role (TOP, JUNGLE, MIDDLE, BOTTOM, UTILITY — "UTILITY"
        /// being the support role). Raw Riot value, left untranslated, same convention as
        /// <see cref="LoLChampionStatDto.ChampionName"/>.
        /// </summary>
        public string TeamPosition { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of games played in this role.
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets the number of games won in this role.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the share of games with a resolved role that were played in this role, as a percentage.
        /// </summary>
        public double PlayRate { get; set; }

        /// <summary>
        /// Gets or sets the win rate in this role, as a percentage.
        /// </summary>
        public double WinRate { get; set; }
    }
}
