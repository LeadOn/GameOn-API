// <copyright file="GetAdminDashboardStatsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Administration.Queries.GetAdminDashboardStats
{
    using GameOn.Application.Players.Queries.GetConnectedPlayer;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

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
        /// <param name="mediator">Mediator, injected.</param>
        public GetAdminDashboardStatsQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<AdminDashboardDto?> Handle(GetAdminDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            // First getting user
            var playerInDb = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = request.ConnectedPlayer }, cancellationToken);

            if (playerInDb == null)
            {
                return null;
            }

            var adminDashboard = new AdminDashboardDto
            {
                CurrentUser = playerInDb,
            };

            // Getting counts
            adminDashboard.Platforms = await this.context.Platforms.CountAsync(cancellationToken);
            adminDashboard.Players = await this.context.Players.CountAsync(cancellationToken);
            adminDashboard.Highlights = await this.context.Highlights.CountAsync(cancellationToken);
            adminDashboard.FifaGames = await this.context.FifaGamesPlayed.CountAsync(cancellationToken);
            adminDashboard.Tournaments = await this.context.Tournaments.CountAsync(cancellationToken);
            adminDashboard.Seasons = await this.context.Seasons.CountAsync(cancellationToken);

            return adminDashboard;
        }
    }
}
