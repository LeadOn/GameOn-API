// <copyright file="ConnectedPlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.DTOs
{
    /// <summary>
    /// Connected Player DTO class.
    /// </summary>
    public class ConnectedPlayerDto
    {
        /// <summary>
        /// Gets or sets Keycloak ID.
        /// </summary>
        public string KeycloakId { get; set; } = "Unknown user";

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; } = "john.doe@test.com";

        /// <summary>
        /// Gets or sets First Name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets Last Name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets Preferred username.
        /// </summary>
        public string? PreferredUsername { get; set; }
    }
}
