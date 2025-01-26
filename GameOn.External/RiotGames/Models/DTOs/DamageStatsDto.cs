// <copyright file="DamageStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// DamageStatsDto class.
    /// </summary>
    public class DamageStatsDto
    {
        /// <summary>
        /// Gets or sets magic damage done.
        /// </summary>
        [JsonProperty("magicDamageDone")]
        public int MagicDamageDone { get; set; }

        /// <summary>
        /// Gets or sets magic damage done to champions.
        /// </summary>
        [JsonProperty("magicDamageDoneToChampions")]
        public int MagicDamageDoneToChampions { get; set; }

        /// <summary>
        /// Gets or sets magic damage taken.
        /// </summary>
        [JsonProperty("magicDamageTaken")]
        public int MagicDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets physical damage done.
        /// </summary>
        [JsonProperty("physicalDamageDone")]
        public int PhysicalDamageDone { get; set; }

        /// <summary>
        /// Gets or sets physical damage done to champions.
        /// </summary>
        [JsonProperty("physicalDamageDoneToChampions")]
        public int PhysicalDamageDoneToChampions { get; set; }

        /// <summary>
        /// Gets or sets physical damage taken.
        /// </summary>
        [JsonProperty("physicalDamageTaken")]
        public int PhysicalDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets total damage done.
        /// </summary>
        [JsonProperty("totalDamageDone")]
        public int TotalDamageDone { get; set; }

        /// <summary>
        /// Gets or sets total damage done to champions.
        /// </summary>
        [JsonProperty("totalDamageDoneToChampions")]
        public int TotalDamageDoneToChampions { get; set; }

        /// <summary>
        /// Gets or sets total damage taken.
        /// </summary>
        [JsonProperty("totalDamageTaken")]
        public int TotalDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets true damage done.
        /// </summary>
        [JsonProperty("trueDamageDone")]
        public int TrueDamageDone { get; set; }

        /// <summary>
        /// Gets or sets true damage done to champions.
        /// </summary>
        [JsonProperty("trueDamageDoneToChampions")]
        public int TrueDamageDoneToChampions { get; set; }

        /// <summary>
        /// Gets or sets true damage taken.
        /// </summary>
        [JsonProperty("trueDamageTaken")]
        public int TrueDamageTaken { get; set; }
    }
}
