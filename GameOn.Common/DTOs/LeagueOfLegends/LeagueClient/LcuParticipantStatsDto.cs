// <copyright file="LcuParticipantStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    /// <summary>
    /// LcuParticipantStatsDto class. End of game stats of a participant, as served by the League
    /// client. Only the fields GameOn! persists are declared here; the client sends about a hundred more.
    /// </summary>
    public class LcuParticipantStatsDto
    {
        /// <summary>
        /// Gets or sets kills.
        /// </summary>
        public int Kills { get; set; }

        /// <summary>
        /// Gets or sets deaths.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Gets or sets assists.
        /// </summary>
        public int Assists { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the participant won.
        /// </summary>
        public bool Win { get; set; }

        /// <summary>
        /// Gets or sets champion level at the end of the game.
        /// </summary>
        public int ChampLevel { get; set; }

        /// <summary>
        /// Gets or sets gold earned.
        /// </summary>
        public int GoldEarned { get; set; }

        /// <summary>
        /// Gets or sets first item slot.
        /// </summary>
        public int Item0 { get; set; }

        /// <summary>
        /// Gets or sets second item slot.
        /// </summary>
        public int Item1 { get; set; }

        /// <summary>
        /// Gets or sets third item slot.
        /// </summary>
        public int Item2 { get; set; }

        /// <summary>
        /// Gets or sets fourth item slot.
        /// </summary>
        public int Item3 { get; set; }

        /// <summary>
        /// Gets or sets fifth item slot.
        /// </summary>
        public int Item4 { get; set; }

        /// <summary>
        /// Gets or sets sixth item slot.
        /// </summary>
        public int Item5 { get; set; }

        /// <summary>
        /// Gets or sets trinket slot.
        /// </summary>
        public int Item6 { get; set; }

        /// <summary>
        /// Gets or sets total damage dealt to champions.
        /// </summary>
        public int TotalDamageDealtToChampions { get; set; }

        /// <summary>
        /// Gets or sets physical damage dealt to champions.
        /// </summary>
        public int PhysicalDamageDealtToChampions { get; set; }

        /// <summary>
        /// Gets or sets magic damage dealt to champions.
        /// </summary>
        public int MagicDamageDealtToChampions { get; set; }

        /// <summary>
        /// Gets or sets true damage dealt to champions.
        /// </summary>
        public int TrueDamageDealtToChampions { get; set; }

        /// <summary>
        /// Gets or sets total damage dealt, to anything.
        /// </summary>
        public int TotalDamageDealt { get; set; }

        /// <summary>
        /// Gets or sets physical damage dealt, to anything.
        /// </summary>
        public int PhysicalDamageDealt { get; set; }

        /// <summary>
        /// Gets or sets magic damage dealt, to anything.
        /// </summary>
        public int MagicDamageDealt { get; set; }

        /// <summary>
        /// Gets or sets true damage dealt, to anything.
        /// </summary>
        public int TrueDamageDealt { get; set; }

        /// <summary>
        /// Gets or sets total damage taken.
        /// </summary>
        public int TotalDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets physical damage taken.
        /// </summary>
        public int PhysicalDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets magic damage taken. The client spells this one "magical", unlike the
        /// "magicDamageDealt" above.
        /// </summary>
        public int MagicalDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets true damage taken.
        /// </summary>
        public int TrueDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets minions killed.
        /// </summary>
        public int TotalMinionsKilled { get; set; }

        /// <summary>
        /// Gets or sets neutral monsters killed.
        /// </summary>
        public int NeutralMinionsKilled { get; set; }

        /// <summary>
        /// Gets or sets vision score.
        /// </summary>
        public int VisionScore { get; set; }

        /// <summary>
        /// Gets or sets wards placed.
        /// </summary>
        public int WardsPlaced { get; set; }

        /// <summary>
        /// Gets or sets wards killed.
        /// </summary>
        public int WardsKilled { get; set; }

        /// <summary>
        /// Gets or sets the time (in seconds) the participant spent crowd controlling enemies.
        /// </summary>
        public int TimeCCingOthers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game ended in an early surrender (remake).
        /// </summary>
        public bool GameEndedInEarlySurrender { get; set; }
    }
}
