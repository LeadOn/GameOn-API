// <copyright file="ValGameParticipantStat.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Derived per-participant performance statistics for a Valorant match (1:1 shared key with <see cref="ValGameParticipant"/>).
    /// </summary>
    public class ValGameParticipantStat
    {
        /// <summary>
        /// Gets or sets the Valorant game participant ID (shared primary key / foreign key).
        /// </summary>
        public int ValGameParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds played in the match.
        /// </summary>
        public int RoundsPlayed { get; set; }

        /// <summary>
        /// Gets or sets the Average Combat Score per round: <c>Score / RoundsPlayed</c>.
        /// </summary>
        public double Acs { get; set; }

        /// <summary>
        /// Gets or sets the KDA ratio: <c>(Kills + Assists) / max(Deaths, 1)</c>.
        /// </summary>
        public double Kda { get; set; }

        /// <summary>
        /// Gets or sets the K/D ratio: <c>Kills / max(Deaths, 1)</c>.
        /// </summary>
        public double KdRatio { get; set; }

        /// <summary>
        /// Gets or sets the kill participation percentage: <c>100 * (Kills + Assists) / teamKills</c>.
        /// </summary>
        public double KillParticipationPercent { get; set; }

        /// <summary>
        /// Gets or sets the Average Damage per Round: <c>DamageDealt / RoundsPlayed</c>.
        /// </summary>
        public double Adr { get; set; }

        /// <summary>
        /// Gets or sets the Damage Delta per Round: <c>(DamageDealt - DamageReceived) / RoundsPlayed</c>.
        /// </summary>
        public double DamageDeltaPerRound { get; set; }

        /// <summary>
        /// Gets or sets the headshot percentage: <c>100 * Headshots / (Headshots + Bodyshots + Legshots)</c>.
        /// </summary>
        public double HeadshotPercent { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds where this participant scored the first kill of the round.
        /// </summary>
        public int FirstKills { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds where this participant was the first player killed in the round.
        /// </summary>
        public int FirstDeaths { get; set; }

        /// <summary>
        /// Gets or sets the KAST percentage: share of rounds where the participant got a Kill, Assist, Survived, or was Traded within 5 seconds.
        /// </summary>
        public double KastPercent { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds in which the participant scored exactly 2 kills.
        /// </summary>
        public int DoubleKills { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds in which the participant scored exactly 3 kills.
        /// </summary>
        public int TripleKills { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds in which the participant scored exactly 4 kills.
        /// </summary>
        public int QuadraKills { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds in which the participant scored 5 or more kills (or earned CeremonyAce).
        /// </summary>
        public int Aces { get; set; }

        /// <summary>
        /// Gets or sets the number of clutch rounds won by this participant.
        /// </summary>
        public int ClutchesWon { get; set; }

        /// <summary>
        /// Gets or sets the number of spikes planted by this participant.
        /// </summary>
        public int SpikePlants { get; set; }

        /// <summary>
        /// Gets or sets the number of spikes defused by this participant.
        /// </summary>
        public int SpikeDefuses { get; set; }

        /// <summary>
        /// Gets or sets the number of melee (knife) kills scored by this participant.
        /// </summary>
        public int KnifeKills { get; set; }

        /// <summary>
        /// Gets or sets the Economy Rating: damage dealt per 1,000 credits spent (<c>DamageDealt / max(SpentOverall, 1) * 1000</c>).
        /// </summary>
        public double EconRating { get; set; }

        /// <summary>
        /// Gets or sets the overall GameOn! performance rating out of 10, blending ACS, ADR, KAST%, KDA, First Kill differential, and Headshot%.
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when these stats were last computed.
        /// </summary>
        public DateTime ComputedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the participant these stats belong to.
        /// </summary>
        [JsonIgnore]
        public virtual ValGameParticipant Participant { get; set; } = null!;
    }
}
