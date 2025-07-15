// <copyright file="UpdatePlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    /// <summary>
    /// Update user DTO.
    /// </summary>
    public class UpdatePlayerDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets Keycloak ID.
        /// </summary>
        public string? KeycloakId { get; set; }

        /// <summary>
        /// Gets or sets Riot Games Nickname.
        /// </summary>
        public string? RiotGamesNickname { get; set; }

        /// <summary>
        /// Gets or sets Riot Games Tag Line.
        /// </summary>
        public string? RiotGamesTagLine { get; set; }

        /// <summary>
        /// Gets or sets Full Name.
        /// </summary>
        public string FullName { get; set; } = "INVALID NICKNAME";

        /// <summary>
        /// Gets or sets Nickname.
        /// </summary>
        public string Nickname { get; set; } = "INVALID FULLNAME";

        /// <summary>
        /// Gets or sets a value indicating whether the player is archived.
        /// </summary>
        public bool Archived { get; set; } = false;
    }
}
