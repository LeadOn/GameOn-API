// <copyright file="GetAdminDashboardStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Administration.Queries.GetAdminDashboardStats
{
    using MediatR;
    using YuGames.Common.DTOs;

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
