// <copyright file="GetAllLeaguePlayersQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetAllLeaguePlayers
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllLeaguePlayersQuery class.
    /// </summary>
    public class GetAllLeaguePlayersQuery : IRequest<IEnumerable<PlayerDto>>
    {
        /// <summary>
        /// Gets or sets a value indicating whether a player is archived or not.
        /// </summary>
        public bool Archived { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether smurf accounts are listed alongside the members they
        /// belong to. On by default: each account holds its own rank, and this list is a ladder of ranks.
        /// Entries carry <see cref="PlayerDto.PrimaryPlayerId"/>, so a caller can nest them under their
        /// owner, or ask for primary accounts only by turning this off.
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;
    }
}
