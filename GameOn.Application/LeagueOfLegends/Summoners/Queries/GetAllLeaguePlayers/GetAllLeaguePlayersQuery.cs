// <copyright file="GetAllLeaguePlayersQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetAllLeaguePlayers
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllLeaguePlayersQuery class.
    /// </summary>
    public class GetAllLeaguePlayersQuery : IRequest<IEnumerable<Player>>
    {
        /// <summary>
        /// Gets or sets a value indicating whether a player is archived or not.
        /// </summary>
        public bool Archived { get; set; } = false;
    }
}
