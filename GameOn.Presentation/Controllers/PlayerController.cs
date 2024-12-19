// <copyright file="PlayerController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers
{
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummoner;
    using GameOn.Application.LeagueOfLegends.Summoners.Queries.GetAllLeaguePlayers;
    using GameOn.Application.LeagueOfLegends.Summoners.Queries.GetLeaguePlayerById;
    using GameOn.Application.Players.Commands.UpdateConnectedPlayer;
    using GameOn.Application.Players.Commands.UpdatePlayer;
    using GameOn.Application.Players.Queries.GetAllPlayers;
    using GameOn.Application.Players.Queries.GetConnectedPlayer;
    using GameOn.Application.Players.Queries.GetPlayerById;
    using GameOn.Application.Players.Queries.GetPlayerStats;
    using GameOn.Application.Stats.Queries.GetGlobalStats;
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Player Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public PlayerController(ISender mediator)
        {
            this.mediator = mediator;
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
        public async Task<IActionResult> GetConnected()
        {
            return this.Ok(await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() }));
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
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = id });

            if (playerInDb is not null)
            {
                return this.Ok(playerInDb);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get a player by its ID.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <returns>200 OK with Player if found, 404 if not found.</returns>
        [HttpGet]
        [Route("{id:int}/lol")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a summoner by its ID.", Description = "Get a summoner by its ID, and retrieve its information.")]
        [SwaggerResponse(200, "Summoner is found.", typeof(PlayerDto))]
        [SwaggerResponse(404, "Player not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetSummonerById(int id)
        {
            var playerInDb = await this.mediator.Send(new GetLeaguePlayerByIdQuery { PlayerId = id });

            if (playerInDb is not null)
            {
                return this.Ok(playerInDb);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get all players in database.
        /// </summary>
        /// <param name="archived">If true, get archived players.</param>
        /// <returns>200 OK with Player list.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all players in database.")]
        [SwaggerResponse(200, "Players in database.", typeof(List<Player>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll(bool? archived)
        {
            return this.Ok(await this.mediator.Send(new GetAllPlayersQuery { Archived = archived ?? false }));
        }

        /// <summary>
        /// Get all league of legends players in database.
        /// </summary>
        /// <param name="archived">If true, get archived players.</param>
        /// <returns>200 OK with Player list.</returns>
        [HttpGet]
        [Route("lol")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all League of Legends players in database.")]
        [SwaggerResponse(200, "Players in database.", typeof(List<Player>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAllLeaguePlayers(bool? archived)
        {
            return this.Ok(await this.mediator.Send(new GetAllLeaguePlayersQuery { Archived = archived ?? false }));
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
            return this.Ok(await this.mediator.Send(new GetPlayerStatsQuery { PlayerId = playerId, SeasonId = seasonId }));
        }

        /// <summary>
        /// Get global stats.
        /// </summary>
        /// <returns>200 OK with Global stats.</returns>
        [HttpGet]
        [Route("global/stats")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get global stats.", Description = "Get global stats.")]
        [SwaggerResponse(200, "Global Stats.", typeof(GlobalStatsDto))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetGlobalStats()
        {
            return this.Ok(await this.mediator.Send(new GetGlobalStatsQuery()));
        }

        /// <summary>
        /// Update user (admin only).
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
            await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            update.KeycloakId = this.User.GetConnectedPlayer().KeycloakId;

            return this.Ok(await this.mediator.Send(new UpdateConnectedPlayerCommand { Player = update }));
        }

        /// <summary>
        /// Update summoner of connected player.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize]
        [Route("me/summoner")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update current user League of Legends summoner.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RefreshUserLoLSummoner()
        {
            var playerInDb = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return this.Ok(await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = playerInDb }));
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.
        }

        /// <summary>
        /// Update summoner of connected player.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize]
        [Route("{id:int}/summoner")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update user League of Legends summoner.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RefreshUserLoLSummonerById(int id)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = id });

#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return this.Ok(await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = playerInDb }));
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.
        }

        /// <summary>
        /// Update connected user.
        /// </summary>
        /// <param name="update"><see cref="UpdatePlayerDto"/>.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize(Roles = "gameon_admin")]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update user profile (Admin only).", Description = "Updates user profile in database, only for admins.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdatePlayerDto update)
        {
            return this.Ok(await this.mediator.Send(new UpdatePlayerCommand { Player = update }));
        }
    }
}
