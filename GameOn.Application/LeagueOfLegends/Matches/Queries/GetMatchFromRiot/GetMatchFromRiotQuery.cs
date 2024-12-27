// <copyright file="GetMatchFromRiotQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetMatchFromRiot
{
    using GameOn.External.RiotGames.Models.DTOs;
    using MediatR;

    /// <summary>
    /// GetMatchFromRiotQuery class.
    /// </summary>
    public class GetMatchFromRiotQuery : IRequest<MatchDto>
    {
        /// <summary>
        /// Gets or sets match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;
    }
}
