// <copyright file="PlayerController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuFoot.Business.Contracts;
    using YuFoot.DTOs;
    using YuFoot.Entities;
    using YuFoot.WebAPI.Classes;
    using YuFoot.WebAPI.Models;

    /// <summary>
    /// Player Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> logger;
        private IPlayerBusiness playerBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="logger">Logger interface (injected).</param>
        /// <param name="player">Player business interface (injected).</param>
        public PlayerController(ILogger<PlayerController> logger, IPlayerBusiness player)
        {
            this.logger = logger;
            this.playerBusi = player;
        }

        /// <summary>
        /// Gets connected user from Database.
        /// </summary>
        /// <returns>200 OK with user account, 401 if not authorized, 500 if something bad happens.</returns>
        [HttpGet]
        [Authorize]
        [Route("me")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Gets current user profile.", Description = "Gets current user profile as stored in database. If the user account doesn't exists, it creates it automatically.")]
        [SwaggerResponse(200, "Current user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetConnectedUser()
        {
            var connectedPlayer = new ConnectedPlayerDto
            {
                Email = this.User.GetUserEmail(),
                KeycloakId = this.User.GetUserId(),
                FirstName = this.User.GetUserFirstName(),
                LastName = this.User.GetUserLastName(),
                PreferredUsername = this.User.GetUserPreferredName(),
            };

            return this.Ok(await this.playerBusi.GetConnectedUser(connectedPlayer));
        }

        /// <summary>
        /// Update connected user.
        /// </summary>
        /// <param name="update"><see cref="UpdatePlayerModel"/>.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize]
        [Route("me")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update current user profile.", Description = "Updates current user profile in database.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateConnectedUser([FromBody] UpdatePlayerModel update)
        {
            // Getting connected user
            var connectedPlayer = new ConnectedPlayerDto
            {
                Email = this.User.GetUserEmail(),
                KeycloakId = this.User.GetUserId(),
                FirstName = this.User.GetUserFirstName(),
                LastName = this.User.GetUserLastName(),
                PreferredUsername = this.User.GetUserPreferredName(),
            };

            await this.playerBusi.GetConnectedUser(connectedPlayer);

            return this.Ok(await this.playerBusi.UpdatePlayer(update.FullName, update.Nickname, update.ProfilePictureUrl, connectedPlayer.KeycloakId));
        }

        /// <summary>
        /// Get a player by its ID.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <returns>200 OK with Player if found, 404 if not found.</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a player by its ID.", Description = "Get a player by its ID, and retrieve its information.")]
        [SwaggerResponse(200, "Player is found.", typeof(Player))]
        [SwaggerResponse(404, "Player not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int id)
        {
            var player = await this.playerBusi.GetPlayerById(id);

            if (player is not null)
            {
                return this.Ok(player);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get all players in database.
        /// </summary>
        /// <returns>200 OK with Player list.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all player in database.")]
        [SwaggerResponse(200, "Players in database.", typeof(List<Player>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.playerBusi.GetAll());
        }

        /// <summary>
        /// Get all player stats.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <returns>200 OK with Player list.</returns>
        [HttpGet]
        [Route("{playerId}/stats")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all stats of a user.", Description = "Get all statistics for each platform of a user.")]
        [SwaggerResponse(200, "List of stats.", typeof(List<PlatformStatsDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetPlayerStats(int playerId)
        {
            return this.Ok(await this.playerBusi.GetPlayerStats(playerId));
        }
    }
}