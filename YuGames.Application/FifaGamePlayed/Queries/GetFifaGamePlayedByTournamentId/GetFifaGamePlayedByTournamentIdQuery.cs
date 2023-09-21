// <copyright file="GetFifaGamePlayedByTournamentIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Queries.GetFifaGamePlayedByTournamentId
{
    using MediatR;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

    /// <summary>
    /// GetFifaGamePlayedByTournamentId class.
    /// </summary>
    public class GetFifaGamePlayedByTournamentIdQuery : IRequest<IEnumerable<FifaGamePlayedDto>>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
