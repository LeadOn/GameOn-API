// <copyright file="GameController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuFoot.Business.Contracts;
    using YuFoot.DTOs;
    using YuFoot.WebAPI.Classes;

    /// <summary>
    /// Game Controller.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class GameController : ControllerBase
    {
        private IGamePlayedBusiness gamePlayedBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController"/> class.
        /// </summary>
        /// <param name="gamePlayed">GamePlayed business interface (injected).</param>
        public GameController(IGamePlayedBusiness gamePlayed)
        {
            this.gamePlayedBusi = gamePlayed;
        }

        /// <summary>
        /// Get last games played.
        /// </summary>
        /// <param name="number">Number of data to retrieve.</param>
        /// <returns>200 OK with Game list.</returns>
        [HttpGet]
        [Route("last/{number:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get last games played.", Description = "Get last games played with their players.")]
        [SwaggerResponse(200, "List of games played.", typeof(List<GamePlayedDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLastGamesPlayed(int number)
        {
            return this.Ok(await this.gamePlayedBusi.GetLastGamesPlayed(number));
        }

        /// <summary>
        /// Gets last games played by player.
        /// </summary>
        /// <param name="number">Limit of results.</param>
        /// <param name="playerId">Player ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("last/{number:int}/player/{playerId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get last games played by player.", Description = "Get last games played by player with their team members.")]
        [SwaggerResponse(200, "List of games played.", typeof(List<GamePlayedDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLastGamesPlayedByPlayer(int number, int playerId)
        {
            return this.Ok(await this.gamePlayedBusi.GetLastGamesPlayedByPlayerId(playerId, number));
        }

        /// <summary>
        /// Create game in database.
        /// </summary>
        /// <param name="game">Create game object.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create game in database.")]
        [SwaggerResponse(200, "Created game.", typeof(List<GamePlayedDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto game)
        {
            game.KeycloakId = this.User.GetUserId();
            var gameInDb = await this.gamePlayedBusi.CreateGame(game);

            return gameInDb is null ? this.Problem() : this.Ok(gameInDb);
        }

        /// <summary>
        /// Get game played by ID.
        /// </summary>
        /// <param name="gameId">Game ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("{gameId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get game played by ID.", Description = "Get game played by ID with its team players and highlights.")]
        [SwaggerResponse(200, "Game played.", typeof(GamePlayedDto))]
        [SwaggerResponse(404, "Game not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int gameId)
        {
            var gameInDb = await this.gamePlayedBusi.GetById(gameId);

            if (gameInDb is null)
            {
                return this.NotFound();
            }

            return this.Ok(gameInDb);
        }
    }
}