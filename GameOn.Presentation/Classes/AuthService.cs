﻿// <copyright file="AuthService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Classes
{
    using System.Security.Claims;
    using GameOn.Common.DTOs;

    /// <summary>
    /// AuthService class.
    /// </summary>
    public static class AuthService
    {
        /// <summary>
        /// Get connected player from Token.
        /// </summary>
        /// <param name="user"><see cref="ClaimsPrincipal"/>.</param>
        /// <returns><see cref="ConnectedPlayerDto"/>.</returns>
        public static ConnectedPlayerDto GetConnectedPlayer(this ClaimsPrincipal user)
        {
            return new ConnectedPlayerDto
            {
                Email = user.GetUserEmail(),
                KeycloakId = user.GetUserId(),
                FirstName = user.GetUserFirstName(),
                LastName = user.GetUserLastName(),
                PreferredUsername = user.GetUserPreferredName(),
            };
        }

        /// <summary>
        /// Gets user ID from Token.
        /// </summary>
        /// <param name="user">User identity.</param>
        /// <returns>User ID.</returns>
        public static string GetUserId(this ClaimsPrincipal user)
        {
            var userIdentity = user.Identities.FirstOrDefault();

            // Getting User ID in claims
            var userIdClaim = userIdentity?.Claims.FirstOrDefault(
                x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            return userIdClaim?.Value ?? "INVALID ID";
        }

        /// <summary>
        /// Get user preferred name.
        /// </summary>
        /// <param name="user">User identity.</param>
        /// <returns>User preferred name.</returns>
        public static string? GetUserPreferredName(this ClaimsPrincipal user)
        {
            var userIdentity = user.Identities.FirstOrDefault();

            var userIdClaim = userIdentity?.Claims.FirstOrDefault(x =>
                x.Type == "preferred_username");

            return userIdClaim?.Value ?? null;
        }

        /// <summary>
        /// Get user first name from the token.
        /// </summary>
        /// <param name="user">User identity.</param>
        /// <returns>First name.</returns>
        public static string? GetUserFirstName(this ClaimsPrincipal user)
        {
            var userIdentity = user.Identities.FirstOrDefault();

            // Getting User ID in claims
            var userPreferredClaim = userIdentity?.Claims.FirstOrDefault(
                x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");

            return userPreferredClaim?.Value
                ?? null;
        }

        /// <summary>
        /// Get user last name from the token.
        /// </summary>
        /// <param name="user">User identity.</param>
        /// <returns>Last name.</returns>
        public static string? GetUserLastName(this ClaimsPrincipal user)
        {
            var userIdentity = user.Identities.FirstOrDefault();

            // Getting User ID in claims
            var userPreferredClaim = userIdentity?.Claims.FirstOrDefault(
                x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");

            return userPreferredClaim?.Value
                ?? null;
        }

        /// <summary>
        /// Get user email from the token.
        /// </summary>
        /// <param name="user">User identity.</param>
        /// <returns>User email.</returns>
        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            var userIdentity = user.Identities.FirstOrDefault();

            var userEmailClaim = userIdentity?.Claims.FirstOrDefault(
                x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

            return userEmailClaim?.Value ?? "INVALID EMAIL";
        }
    }
}