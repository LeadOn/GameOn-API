// <copyright file="GamePlayed.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Entities
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// GamePlayed class.
    /// </summary>
    public class GamePlayed
    {
        /// <summary>
        /// Gets or sets player's ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets game date.
        /// </summary>
        public DateTime PlayedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Team Code 1.
        /// </summary>
        public string? TeamCode1 { get; set; }

        /// <summary>
        /// Gets or sets Team Code 2.
        /// </summary>
        public string? TeamCode2 { get; set; }

        /// <summary>
        /// Gets or sets team 1 score.
        /// </summary>
        public int TeamScore1 { get; set; }

        /// <summary>
        /// Gets or sets team 2 score.
        /// </summary>
        public int TeamScore2 { get; set; }

        /// <summary>
        /// Gets or sets Platform ID.
        /// </summary>
        public int? PlatformId { get; set; }

        /// <summary>
        /// Gets or sets TeamPlayers.
        /// </summary>
        [JsonIgnore]
        public virtual List<TeamPlayer> TeamPlayers { get; set; } = null!;

        /// <summary>
        /// Gets or sets Platform.
        /// </summary>
        [JsonIgnore]
        public virtual Platform? Platform { get; set; } = null!;

        /// <summary>
        /// Gets or sets Highlights.
        /// </summary>
        [JsonIgnore]
        public virtual List<Highlight> Highlights { get; set; } = null!;
    }
}