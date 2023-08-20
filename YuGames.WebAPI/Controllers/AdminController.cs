// <copyright file="AdminController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Business.Contracts;
    using YuGames.DTOs;
    using YuGames.Entities;
    using YuGames.WebAPI.Classes;

    /// <summary>
    /// Admin Controller.
    /// </summary>
    [ApiController]
    [Authorize(Roles = "yugames_admin")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AdminController : ControllerBase
    {
        private IAdminBusiness adminBusi;
        private IPlayerBusiness playerBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="adminBusi">Admin business, injected.</param>
        /// <param name="playerBusi">Player business, injected.</param>
        public AdminController(IAdminBusiness adminBusi, IPlayerBusiness playerBusi)
        {
            this.adminBusi = adminBusi;
            this.playerBusi = playerBusi;
        }

        /// <summary>
        /// Gets admin dashboard stats.
        /// </summary>
        /// <returns>IActionResult objects.</returns>
        [HttpGet]
        [Authorize]
        [Route("dashboard")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Gets administrator dashboard.")]
        [SwaggerResponse(200, "Administrator dashboard.", typeof(AdminDashboardDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var connectedPlayer = new ConnectedPlayerDto
            {
                Email = this.User.GetUserEmail(),
                KeycloakId = this.User.GetUserId(),
                FirstName = this.User.GetUserFirstName(),
                LastName = this.User.GetUserLastName(),
                PreferredUsername = this.User.GetUserPreferredName(),
            };

            var userInDb = await this.playerBusi.GetConnectedUser(connectedPlayer);

            if (userInDb is null)
            {
                return this.Problem();
            }

#pragma warning disable CS8604 // Possible null reference argument.
            var adminDashboard = await this.adminBusi.GetDashboardStats(userInDb.KeycloakId);
#pragma warning restore CS8604 // Possible null reference argument.

            if (adminDashboard is null)
            {
                return this.Problem();
            }

            return this.Ok(adminDashboard);
        }
    }
}