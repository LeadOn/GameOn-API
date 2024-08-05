// <copyright file="GetPlayerByEmailQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Queries.GetPlayerByEmail
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetPlayerByEmailQuery class.
    /// </summary>
    public class GetPlayerByEmailQuery : IRequest<Player?>
    {
        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; } = "default@exemple.com";
    }
}
