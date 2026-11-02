// <copyright file="ValGameParticipantPerformance.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Riot's own performance rating of a participant in a Valorant match. Riot ships it under provisional
    /// field names (<c>TempValue*</c>), so it may change or vanish without notice: never rank players on it.
    /// </summary>
    public class ValGameParticipantPerformance
    {
        /// <summary>
        /// Gets or sets the participant ID (shared primary key).
        /// </summary>
        public int ValGameParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the overall performance score.
        /// </summary>
        public double? Score { get; set; }

        /// <summary>
        /// Gets or sets the overall grade (e.g. pass).
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// Gets or sets the kill impact rating (double_down, down, neutral, up or double_up).
        /// </summary>
        public string? CombatKillImpact { get; set; }

        /// <summary>
        /// Gets or sets the death impact rating.
        /// </summary>
        public string? CombatDeathImpact { get; set; }

        /// <summary>
        /// Gets or sets the damage rating.
        /// </summary>
        public string? CombatDamage { get; set; }

        /// <summary>
        /// Gets or sets the trades rating.
        /// </summary>
        public string? CombatTrades { get; set; }

        /// <summary>
        /// Gets or sets the assists rating.
        /// </summary>
        public string? UtilityAssists { get; set; }

        /// <summary>
        /// Gets or sets the spike plants rating.
        /// </summary>
        public string? UtilityPlants { get; set; }

        /// <summary>
        /// Gets or sets the spike defuses rating.
        /// </summary>
        public string? UtilityDefuses { get; set; }

        /// <summary>
        /// Gets or sets the utility usage rating.
        /// </summary>
        public string? UtilityUsage { get; set; }

        /// <summary>
        /// Gets or sets the adjusted kills per round.
        /// </summary>
        public double? AdjustedKills { get; set; }

        /// <summary>
        /// Gets or sets the adjusted deaths per round (negative).
        /// </summary>
        public double? AdjustedDeaths { get; set; }

        /// <summary>
        /// Gets or sets the assists per round.
        /// </summary>
        public double? Assists { get; set; }

        /// <summary>
        /// Gets or sets the damage per round.
        /// </summary>
        public double? Damage { get; set; }

        /// <summary>
        /// Gets or sets the trades per round.
        /// </summary>
        public double? Trades { get; set; }

        /// <summary>
        /// Gets or sets the utility usage per round.
        /// </summary>
        public double? UtilityUsagePerRound { get; set; }

        /// <summary>
        /// Gets or sets the spike plants per round.
        /// </summary>
        public double? Plants { get; set; }

        /// <summary>
        /// Gets or sets the spike defuses per round.
        /// </summary>
        public double? Defuses { get; set; }

        /// <summary>
        /// Gets or sets the participant.
        /// </summary>
        [JsonIgnore]
        public virtual ValGameParticipant Participant { get; set; } = null!;
    }
}
