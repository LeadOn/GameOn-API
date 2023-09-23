// <copyright file="GetMostWinsFifaTeamsByPlayerQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaTeams.Queries.GetMostWinsFifaTeamsByPlayer
{
    using MediatR;
    using YuGames.Common.DTOs;

    /// <summary>
    /// GetMostWinsFifaTeamsByPlayerQuery class.
    /// </summary>
    public class GetMostWinsFifaTeamsByPlayerQuery : IRequest<List<TopTeamStatDto>>
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
