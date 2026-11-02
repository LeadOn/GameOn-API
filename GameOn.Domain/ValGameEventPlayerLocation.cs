// <copyright file="ValGameEventPlayerLocation.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Where one player stood, and where they looked, when a kill, a plant or a defuse happened in a Valorant match.
    /// </summary>
    public class ValGameEventPlayerLocation
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
        /// Gets or sets the 0-based round index.
        /// </summary>
        public int RoundNumber { get; set; }

        /// <summary>
        /// Gets or sets the event type: Kill, Plant or Defuse.
        /// </summary>
        public string EventType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the kill this snapshot belongs to, null for a plant or a defuse.
        /// </summary>
        public int? ValGameKillId { get; set; }

        /// <summary>
        /// Gets or sets the player's Valorant PUUID.
        /// </summary>
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the player's team (Red or Blue).
        /// </summary>
        public string? Team { get; set; }

        /// <summary>
        /// Gets or sets the X game coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y game coordinate.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the view direction, in radians.
        /// </summary>
        public double ViewRadians { get; set; }

        /// <summary>
        /// Gets or sets the parent match.
        /// </summary>
        [JsonIgnore]
        public virtual ValGame Game { get; set; } = null!;

        /// <summary>
        /// Gets or sets the kill this snapshot belongs to.
        /// </summary>
        [JsonIgnore]
        public virtual ValGameKill? Kill { get; set; }
    }
}
