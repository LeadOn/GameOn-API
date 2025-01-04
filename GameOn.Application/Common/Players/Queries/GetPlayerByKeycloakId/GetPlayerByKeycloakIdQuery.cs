// <copyright file="GetPlayerByKeycloakIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Queries.GetPlayerByKeycloakId
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetPlayerByKeycloakIdQuery class.
    /// </summary>
    public class GetPlayerByKeycloakIdQuery : IRequest<Player?>
    {
        /// <summary>
        /// Gets or sets Keycloak ID.
        /// </summary>
        public string KeycloakId { get; set; } = string.Empty;
    }
}
