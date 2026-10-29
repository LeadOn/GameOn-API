// <copyright file="ValGameKill.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Kill timeline event in a Valorant match.
    /// </summary>
    public class ValGameKill
    {
        /// <summary>
        /// Gets or sets the primary key ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent match UUID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the 0-based round index in which the kill occurred.
        /// </summary>
        public int RoundNumber { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time within the round in milliseconds.
        /// </summary>
        public int TimeInRoundMs { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time within the match in milliseconds.
        /// </summary>
        public int TimeInMatchMs { get; set; }

        /// <summary>
        /// Gets or sets the killer's Valorant PUUID.
        /// </summary>
        public string? KillerPuuid { get; set; }

        /// <summary>
        /// Gets or sets the killer's team identifier.
        /// </summary>
        public string? KillerTeam { get; set; }

        /// <summary>
        /// Gets or sets the victim's Valorant PUUID.
        /// </summary>
        public string? VictimPuuid { get; set; }

        /// <summary>
        /// Gets or sets the victim's team identifier.
        /// </summary>
        public string? VictimTeam { get; set; }

        /// <summary>
        /// Gets or sets the number of assisting players on the kill.
        /// </summary>
        public int AssistantsCount { get; set; }

        /// <summary>
        /// Gets or sets the comma-separated Valorant PUUIDs of assisting players.
        /// </summary>
        public string? AssistantPuuids { get; set; }

        /// <summary>
        /// Gets or sets the weapon or ability UUID used to score the kill.
        /// </summary>
        public string? WeaponId { get; set; }

        /// <summary>
        /// Gets or sets the weapon or ability display name (e.g. "Vandal", "Phantom", "Melee").
        /// </summary>
        public string? WeaponName { get; set; }

        /// <summary>
        /// Gets or sets the damage source type (e.g. "Weapon", "Melee", "Ability", "Bomb").
        /// </summary>
        public string? WeaponType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether secondary fire (ADS / right-click) was used.
        /// </summary>
        public bool SecondaryFireMode { get; set; }

        /// <summary>
        /// Gets or sets the parent match.
        /// </summary>
        [JsonIgnore]
        public virtual ValGame Game { get; set; } = null!;
    }
}
