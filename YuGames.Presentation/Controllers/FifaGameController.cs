// <copyright file="FifaGameController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.FifaGamePlayed.Queries.GetFifaGamePlayedById;
    using YuGames.Application.FifaGamePlayed.Queries.GetFifaGamePlayedByTournamentId;
    using YuGames.Application.FifaGamePlayed.Queries.GetLastFifaGamesPlayed;
    using YuGames.Common.DTOs;

    /// <summary>
    /// FifaGame Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class FifaGameController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FifaGameController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public FifaGameController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets tournament games.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("tournament/{tournamentId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get games played by tournament.")]
        [SwaggerResponse(200, "List of games played.", typeof(List<FifaGamePlayedDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetTournamentGames(int tournamentId)
        {
            return this.Ok(await this.mediator.Send(new GetFifaGamePlayedByTournamentIdQuery { TournamentId = tournamentId }));
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
        [SwaggerResponse(200, "Game played.", typeof(FifaGamePlayedDto))]
        [SwaggerResponse(404, "Game not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int gameId)
        {
            var gameInDb = await this.mediator.Send(new GetFifaGamePlayedByIdQuery { FifaGamePlayedId = gameId });

            if (gameInDb is null)
            {
                return this.NotFound();
            }

            return this.Ok(gameInDb);
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
        [SwaggerResponse(200, "List of games played.", typeof(List<FifaGamePlayedDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLastGamesPlayed(int number)
        {
            return this.Ok(await this.mediator.Send(new GetLastFifaGamesPlayedQuery { Limit = number }));
        }
    }
}
