// <copyright file="AdminController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.Administration.Queries.GetAdminDashboardStats;
    using YuGames.Application.Players.Queries.GetConnectedPlayer;
    using YuGames.Common.DTOs;
    using YuGames.Presentation.Classes;

    /// <summary>
    /// Admin Controller.
    /// </summary>
    [ApiController]
    [Authorize(Roles = "yugames_admin")]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public AdminController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets admin dashboard stats.
        /// </summary>
        /// <returns>IActionResult objects.</returns>
        [HttpGet]
        [Route("dashboard")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Gets administrator dashboard.")]
        [SwaggerResponse(200, "Administrator dashboard.", typeof(AdminDashboardDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAdminDashboard()
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var adminDashboard = await this.mediator.Send(new GetAdminDashboardStatsQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });
#pragma warning restore CS8604 // Possible null reference argument.

            if (adminDashboard is null)
            {
                return this.Problem();
            }

            return this.Ok(adminDashboard);
        }
    }
}
