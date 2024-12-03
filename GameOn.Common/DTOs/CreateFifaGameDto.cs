// <copyright file="CreateFifaGameDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    /// <summary>
    /// CreateFifaGameDto class.
    /// </summary>
    public class CreateFifaGameDto
    {
        /// <summary>
        /// Gets or sets date.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets Keycloak ID.
        /// </summary>
        public string KeycloakId { get; set; } = "ZEIFEZF";

        /// <summary>
        /// Gets or sets Platform Id.
        /// </summary>
        public int PlatformId { get; set; }

        /// <summary>
        /// Gets or sets Team 1 players IDs.
        /// </summary>
        public List<string> Team1 { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets FifaTeam 1.
        /// </summary>
        public int? FifaTeam1 { get; set; }

        /// <summary>
        /// Gets or sets Team 2 players IDs.
        /// </summary>
        public List<string> Team2 { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets FifaTeam 2.
        /// </summary>
        public int? FifaTeam2 { get; set; }

        /// <summary>
        /// Gets or sets Team score 1.
        /// </summary>
        public int TeamScore1 { get; set; }

        /// <summary>
        /// Gets or sets Team score 2.
        /// </summary>
        public int TeamScore2 { get; set; }
    }
}