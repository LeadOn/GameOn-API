// <copyright file="GamePlayedDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>
namespace YuFoot.DTOs
{
    using YuFoot.Entities;

    /// <summary>
    /// GamePlayedDto class.
    /// </summary>
    public class GamePlayedDto
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Played On.
        /// </summary>
        public DateTime PlayedOn { get; set; }

        /// <summary>
        /// Gets or sets Platform.
        /// </summary>
        public string? Platform { get; set; }

        /// <summary>
        /// Gets or sets Team 1.
        /// </summary>
        public TeamDto Team1 { get; set; } = new TeamDto();

        /// <summary>
        /// Gets or sets Team 2.
        /// </summary>
        public TeamDto Team2 { get; set; } = new TeamDto();

        /// <summary>
        /// Gets or sets Highlights.
        /// </summary>
        public List<Highlight> Highlights { get; set; } = new List<Highlight>();
    }
}