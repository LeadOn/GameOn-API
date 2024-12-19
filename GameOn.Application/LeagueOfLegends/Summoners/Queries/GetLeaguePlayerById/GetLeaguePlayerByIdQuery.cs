// <copyright file="GetLeaguePlayerByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetLeaguePlayerById
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetLeaguePlayerByIdQuery class.
    /// </summary>
    public class GetLeaguePlayerByIdQuery : IRequest<PlayerDto?>
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
