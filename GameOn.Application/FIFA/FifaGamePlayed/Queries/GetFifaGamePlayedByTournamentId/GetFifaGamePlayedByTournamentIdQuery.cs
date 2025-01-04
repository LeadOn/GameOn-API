// <copyright file="GetFifaGamePlayedByTournamentIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaGamePlayed.Queries.GetFifaGamePlayedByTournamentId
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetFifaGamePlayedByTournamentId class.
    /// </summary>
    public class GetFifaGamePlayedByTournamentIdQuery : IRequest<IEnumerable<FifaGamePlayedDto>>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether you get planned games or not.
        /// </summary>
        public bool IsPlayed { get; set; }
    }
}
