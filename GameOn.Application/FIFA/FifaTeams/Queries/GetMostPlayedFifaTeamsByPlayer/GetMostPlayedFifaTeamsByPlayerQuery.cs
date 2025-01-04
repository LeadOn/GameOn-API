// <copyright file="GetMostPlayedFifaTeamsByPlayerQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaTeams.Queries.GetMostPlayedFifaTeamsByPlayer
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetMostPlayedFifaTeamsByPlayerQuery class.
    /// </summary>
    public class GetMostPlayedFifaTeamsByPlayerQuery : IRequest<List<TopTeamStatDto>>
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets Number of Teams.
        /// </summary>
        public int NumberOfTeams { get; set; }

        /// <summary>
        /// Gets or sets Season ID.
        /// </summary>
        public int SeasonId { get; set; }
    }
}
