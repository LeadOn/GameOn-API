﻿// <copyright file="GetAdminDashboardStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Administration.Queries.GetAdminDashboardStats
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetAdminDashboardStatsQuery class.
    /// </summary>
    public class GetAdminDashboardStatsQuery : IRequest<AdminDashboardDto?>
    {
        /// <summary>
        /// Gets or sets Connected Player.
        /// </summary>
        public ConnectedPlayerDto ConnectedPlayer { get; set; } = new ConnectedPlayerDto();
    }
}
