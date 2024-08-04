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
        /// Gets or sets Full Name.
        /// </summary>
        public string FullName { get; set; } = "INVALID NICKNAME";

        /// <summary>
        /// Gets or sets Nickname.
        /// </summary>
        public string Nickname { get; set; } = "INVALID FULLNAME";

        /// <summary>
        /// Gets or sets Profile Picture URL.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = "INVALID URL";
    }
}
