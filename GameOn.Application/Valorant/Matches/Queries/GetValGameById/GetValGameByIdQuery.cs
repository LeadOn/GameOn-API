// <copyright file="GetValGameByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Queries.GetValGameById
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// Retrieves a single Valorant match with its participants, derived stats, and teams.
    /// </summary>
    public class GetValGameByIdQuery : IRequest<ValGame?>
    {
        /// <summary>
        /// Gets or sets the Valorant match UUID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;
    }
}
