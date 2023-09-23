﻿// <copyright file="GetPlayerStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Players.Queries.GetPlayerStats
{
    using MediatR;
    using YuGames.Common.DTOs;

    /// <summary>
    /// GetAllPlayersQuery class.
    /// </summary>
    public class GetPlayerStatsQuery : IRequest<FifaPlayerStatsDto>
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets Season ID.
        /// </summary>
        public int? SeasonId { get; set; }
    }
}
