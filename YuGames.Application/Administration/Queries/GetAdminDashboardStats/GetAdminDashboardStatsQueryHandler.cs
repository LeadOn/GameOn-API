// <copyright file="GetAdminDashboardStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Administration.Queries.GetAdminDashboardStats
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Application.Players.Queries.GetConnectedPlayer;
    using YuGames.Common.DTOs;

    /// <summary>
    /// GetCurrentSeasonQueryHandler class.
    /// </summary>
    public class GetAdminDashboardStatsQueryHandler : IRequestHandler<GetAdminDashboardStatsQuery, AdminDashboardDto?>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAdminDashboardStatsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAdminDashboardStatsQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<AdminDashboardDto?> Handle(GetAdminDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            // First getting user
            var playerInDb = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = request.ConnectedPlayer });

            if (playerInDb == null)
            {
                return null;
            }

            var adminDashboard = new AdminDashboardDto
            {
                CurrentUser = playerInDb,
            };

            // Getting counts
            adminDashboard.Platforms = await this.context.Platforms.CountAsync();
            adminDashboard.Players = await this.context.Players.CountAsync();
            adminDashboard.Highlights = await this.context.Highlights.CountAsync();
            adminDashboard.FifaGames = await this.context.FifaGamesPlayed.CountAsync();
            adminDashboard.Tournaments = await this.context.Tournaments.CountAsync();
            adminDashboard.Seasons = await this.context.Seasons.CountAsync();

            return adminDashboard;
        }
    }
}
