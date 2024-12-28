// <copyright file="LoLGameParticipant.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// LoLGameParticipant class.
    /// </summary>
    public class LoLGameParticipant : ParticipantDto
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Player.
        /// </summary>
        public virtual Player Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets Game.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGame Game { get; set; } = null!;
    }
}