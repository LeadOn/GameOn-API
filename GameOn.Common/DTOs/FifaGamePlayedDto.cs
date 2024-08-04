// <copyright file="FifaGamePlayedDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Domain;

    /// <summary>
    /// FifaGamePlayedDto class.
    /// </summary>
    public class FifaGamePlayedDto
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Created By.
        /// </summary>
        public Player CreatedBy { get; set; } = new Player();

        /// <summary>
        /// Gets or sets Played On.
        /// </summary>
        public DateTime PlayedOn { get; set; }

        /// <summary>
        /// Gets or sets Platform.
        /// </summary>
        public string? Platform { get; set; }

        /// <summary>
        /// Gets or sets Platform ID.
        /// </summary>
        public int? PlatformId { get; set; }

        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int? TournamentId { get; set; }

        /// <summary>
        /// Gets or sets Phase.
        /// </summary>
        public int? Phase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether game is played or not.
        /// </summary>
        public bool IsPlayed { get; set; }

        /// <summary>
        /// Gets or sets Team 1.
        /// </summary>
        public FifaTeamDto Team1 { get; set; } = new FifaTeamDto();

        /// <summary>
        /// Gets or sets Team 2.
        /// </summary>
        public FifaTeamDto Team2 { get; set; } = new FifaTeamDto();

        /// <summary>
        /// Gets or sets Season.
        /// </summary>
        public Season Season { get; set; } = new Season();

        /// <summary>
        /// Gets or sets Highlights.
        /// </summary>
        public List<Highlight> Highlights { get; set; } = new List<Highlight>();
    }
}