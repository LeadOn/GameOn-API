// <copyright file="GetGameByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetGameById
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetGameByIdQuery class.
    /// </summary>
    public class GetGameByIdQuery : IRequest<LoLGame?>
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;
    }
}
