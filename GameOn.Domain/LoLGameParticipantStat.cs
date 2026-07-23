// <copyright file="LoLGameParticipantStat.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGameParticipantStat class. Derived performance stats computed from a participant's raw
    /// data, its last timeline frame, and its match's timeline events.
    /// </summary>
    public class LoLGameParticipantStat
    {
        /// <summary>
        /// Gets or sets the LoL Game Participant ID (shared primary key / foreign key).
        /// </summary>
        public int LoLGameParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the game duration, in seconds.
        /// </summary>
        public int GameDurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets the KDA ratio: (Kills + Assists) / max(Deaths, 1).
        /// </summary>
        public double Kda { get; set; }

        /// <summary>
        /// Gets or sets the kill participation percentage: 100 * (Kills + Assists) / team kills.
        /// </summary>
        public double KillParticipationPercent { get; set; }

        /// <summary>
        /// Gets or sets the creep score (minions + jungle monsters killed).
        /// </summary>
        public int CreepScore { get; set; }

        /// <summary>
        /// Gets or sets the creep score per minute.
        /// </summary>
        public double CsPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the total gold earned.
        /// </summary>
        public int GoldEarned { get; set; }

        /// <summary>
        /// Gets or sets the gold earned per minute.
        /// </summary>
        public double GoldPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the total damage dealt to champions.
        /// </summary>
        public int DamageDealtToChampions { get; set; }

        /// <summary>
        /// Gets or sets the damage dealt to champions per minute.
        /// </summary>
        public double DamagePerMinute { get; set; }

        /// <summary>
        /// Gets or sets the total damage taken.
        /// </summary>
        public int DamageTaken { get; set; }

        /// <summary>
        /// Gets or sets the number of wards placed (vision score proxy).
        /// </summary>
        public int WardsPlaced { get; set; }

        /// <summary>
        /// Gets or sets the number of wards killed (vision score proxy).
        /// </summary>
        public int WardsKilled { get; set; }

        /// <summary>
        /// Gets or sets the date these stats were last computed.
        /// </summary>
        public DateTime ComputedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the participant these stats belong to.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGameParticipant Participant { get; set; } = null!;
    }
}
