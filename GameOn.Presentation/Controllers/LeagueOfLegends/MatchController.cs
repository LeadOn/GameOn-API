// <copyright file="MatchController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.LeagueOfLegends.Matches.Queries.GetGameById;
    using GameOn.Application.LeagueOfLegends.Matches.Queries.GetLastGamesPlayed;
    using GameOn.Application.LeagueOfLegends.Matches.Queries.GetMatchFromRiot;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Models.DTOs;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Match Controller.
    /// </summary>
    [ApiController]
    [Route("lol/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public MatchController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get last matches played by Summoner.
        /// </summary>
        /// <param name="playerId">GameOn! Player ID.</param>
        /// <returns>200 OK with Player list.</returns>
        [HttpGet]
        [Route("player/{playerId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get last games played by summoner (directly from Riot API).")]
        [SwaggerResponse(200, "Riot Games game IDs.", typeof(List<string>))]
        [SwaggerResponse(404, "Player not found / no Riot Games PUUID found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLastGamesForUser(int playerId)
        {
            var lastGames = await this.mediator.Send(new GetLastGamesPlayedQuery { PlayerId = playerId });

            if (lastGames is not null)
            {
                return this.Ok(lastGames);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get match by ID.
        /// </summary>
        /// <param name="gameId">Game ID.</param>
        /// <returns>200 OK with Match.</returns>
        [HttpGet]
        [Route("{gameId:long}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get Match by ID.")]
        [SwaggerResponse(200, "Match.", typeof(LoLGame))]
        [SwaggerResponse(404, "Match not found")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(long gameId)
        {
            return this.Ok(await this.mediator.Send(new GetGameByIdQuery { GameId = gameId }));
        }
    }
}
