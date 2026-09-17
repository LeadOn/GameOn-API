// <copyright file="LcuTeamDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    /// <summary>
    /// LcuTeamDto class. Team objectives of a game, as served by the League client.
    /// </summary>
    public class LcuTeamDto
    {
        /// <summary>
        /// Gets or sets the team ID (100 or 200).
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the outcome, as the literal "Win" or "Fail" (the client doesn't send a boolean here).
        /// </summary>
        public string? Win { get; set; }

        /// <summary>
        /// Gets or sets towers destroyed.
        /// </summary>
        public int TowerKills { get; set; }

        /// <summary>
        /// Gets or sets inhibitors destroyed.
        /// </summary>
        public int InhibitorKills { get; set; }

        /// <summary>
        /// Gets or sets dragons killed.
        /// </summary>
        public int DragonKills { get; set; }

        /// <summary>
        /// Gets or sets rift heralds killed.
        /// </summary>
        public int RiftHeraldKills { get; set; }

        /// <summary>
        /// Gets or sets barons killed.
        /// </summary>
        public int BaronKills { get; set; }

        /// <summary>
        /// Gets or sets voidgrubs killed.
        /// </summary>
        public int HordeKills { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the team drew first blood.
        /// </summary>
        public bool FirstBlood { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the team took the first tower.
        /// </summary>
        public bool FirstTower { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the team took the first inhibitor.
        /// </summary>
        public bool FirstInhibitor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the team took the first dragon. The typo is
        /// Riot's own: the client really does send "firstDargon".
        /// </summary>
        public bool FirstDargon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the team took the first baron.
        /// </summary>
        public bool FirstBaron { get; set; }
    }
}
