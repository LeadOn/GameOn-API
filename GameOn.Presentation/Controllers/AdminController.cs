// <copyright file="AdminController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers
{
    using GameOn.Application.Administration.Queries.GetAdminDashboardStats;
    using GameOn.Application.LeagueOfLegends.Matches.Commands.RecomputeLoLGameParticipantStats;
    using GameOn.Common.DTOs;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Admin Controller.
    /// </summary>
    [ApiController]
    [Authorize(Roles = "gameon_admin")]
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
            var adminDashboard = await this.mediator.Send(new GetAdminDashboardStatsQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (adminDashboard is null)
            {
                return this.Problem();
            }

            return this.Ok(adminDashboard);
        }

        /// <summary>
        /// Recomputes derived performance stats (KDA, CS/min, gold/min, damage/min, kill participation, wards)
        /// for every League of Legends game participant already in database.
        /// </summary>
        /// <returns>200 OK with the number of participants updated.</returns>
        [HttpPost]
        [Route("lol/recompute-participant-stats")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Recompute LoL game participant derived stats for the whole history.")]
        [SwaggerResponse(200, "Number of participants updated.", typeof(int))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RecomputeLoLGameParticipantStats()
        {
            var totalUpdated = await this.mediator.Send(new RecomputeLoLGameParticipantStatsCommand());

            return this.Ok(totalUpdated);
        }
    }
}
