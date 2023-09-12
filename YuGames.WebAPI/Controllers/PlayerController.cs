// <copyright file="PlayerController.cs" company="LeadOn's Corp'">
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
    /// Player Controller.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PlayerController : ControllerBase
    {
        private IPlayerBusiness playerBusi;
        private IFifaTeamBusiness teamBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="player">Player business interface (injected).</param>
        /// <param name="teamBusi">FifaTeam business interface (injected).</param>
        public PlayerController(IPlayerBusiness player, IFifaTeamBusiness teamBusi)
        {
            this.playerBusi = player;
            this.teamBusi = teamBusi;
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
        /// <param name="update"><see cref="UpdatePlayerDto"/>.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize]
        [Route("me")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update current user profile.", Description = "Updates current user profile in database.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateConnectedUser([FromBody] UpdatePlayerDto update)
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
        [Route("{id:int}")]
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
        [Route("all")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all players in database.")]
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
        /// <param name="seasonId">Season ID.</param>
        /// <returns>200 OK with Player list.</returns>
        [HttpGet]
        [Route("{playerId}/stats")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all stats of a user.", Description = "Get all statistics for each platform of a user.")]
        [SwaggerResponse(200, "List of stats.", typeof(FifaPlayerStatsDto))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetPlayerStats(int playerId, int? seasonId)
        {
            return this.Ok(await this.playerBusi.GetPlayerStats(playerId, seasonId));
        }

        /// <summary>
        /// Get most played teams of a user.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="numberOfTeams">Number of Teams.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("{playerId:int}/mostplayedteams")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get most played teams of a user.")]
        [SwaggerResponse(200, "List of most played teams.", typeof(List<TopTeamStatDto>))]
        [SwaggerResponse(404, "Player not found in database.")]
        [SwaggerResponse(500, "Something wrong happened..")]
        public async Task<IActionResult> GetMostPlayedTeams(int playerId, int? numberOfTeams)
        {
            if (numberOfTeams is null)
            {
                numberOfTeams = 3;
            }

            var userInDb = await this.playerBusi.GetPlayerById(playerId);

            if (userInDb is null)
            {
                return this.NotFound();
            }
            else
            {
                var mostPlayedTeams = await this.teamBusi.GetMostPlayedTeams(userInDb.Id, (int)numberOfTeams, int.Parse(Environment.GetEnvironmentVariable("CURRENT_SEASON") ?? "1"));
                return this.Ok(mostPlayedTeams);
            }
        }
    }
}