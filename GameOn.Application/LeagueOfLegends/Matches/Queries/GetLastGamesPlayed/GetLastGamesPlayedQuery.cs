// <copyright file="GetLastGamesPlayedQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetLastGamesPlayed
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetLastGamesPlayedQuery class.
    /// </summary>
    public class GetLastGamesPlayedQuery : IRequest<IEnumerable<string>?>
    {
        /// <summary>
        /// Gets or sets player Id.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
