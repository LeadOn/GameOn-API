// <copyright file="ParticipantFrameDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// ParticipantFrameDto class.
    /// </summary>
    public class ParticipantFrameDto
    {
        /// <summary>
        /// Gets or sets current gold.
        /// </summary>
        [JsonProperty("currentGold")]
        public int CurrentGold { get; set; }

        /// <summary>
        /// Gets or sets damage stats.
        /// </summary>
        [JsonProperty("damageStats")]
        public DamageStatsDto DamageStats { get; set; } = new DamageStatsDto();

        /// <summary>
        /// Gets or sets gold per second.
        /// </summary>
        [JsonProperty("goldPerSecond")]
        public int GoldPerSecond { get; set; }

        /// <summary>
        /// Gets or sets jungleMinionKilled.
        /// </summary>
        [JsonProperty("jungleMinionsKilled")]
        public int JungleMinionsKilled { get; set; }

        /// <summary>
        /// Gets or sets Level.
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets minions killed.
        /// </summary>
        [JsonProperty("minionsKilled")]
        public int MinionsKilled { get; set; }

        /// <summary>
        /// Gets or sets participant ID.
        /// </summary>
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets time enemy spent controlled.
        /// </summary>
        [JsonProperty("timeEnemySpentControlled")]
        public int TimeEnemySpentControlled { get; set; }

        /// <summary>
        /// Gets or sets total gold.
        /// </summary>
        [JsonProperty("totalGold")]
        public int TotalGold { get; set; }

        /// <summary>
        /// Gets or sets xp.
        /// </summary>
        [JsonProperty("xp")]
        public int Xp { get; set; }
    }
}
