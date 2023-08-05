// <copyright file="ConnectedPlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.DTOs
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
        public string Email { get; set; } = "John Doe";
    }
}
