// <copyright file="MatchController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.LeagueOfLegends.Matches.Commands.UpdateLoLGame;
    using GameOn.Application.LeagueOfLegends.Matches.Queries.GetGameById;
    using GameOn.Application.LeagueOfLegends.Matches.Queries.GetLastGamesPlayed;
    using GameOn.Domain;
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
        [SwaggerOperation(Summary = "Get last games played by summoner.")]
        [SwaggerResponse(200, "Games played.", typeof(List<LoLGame>))]
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
        /// <param name="matchId">Match ID.</param>
        /// <returns>200 OK with Match.</returns>
        [HttpGet]
        [Route("{matchId}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get Match by ID.")]
        [SwaggerResponse(200, "Match.", typeof(LoLGame))]
        [SwaggerResponse(404, "Match not found")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(string matchId)
        {
            return this.Ok(await this.mediator.Send(new GetGameByIdQuery { MatchId = matchId }));
        }

        /// <summary>
        /// Update match by ID.
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <returns>200 OK with Match.</returns>
        [HttpPost]
        [Route("{matchId}/update")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update Match information (retrieve data from Riot Games).")]
        [SwaggerResponse(204, "Success.")]
        [SwaggerResponse(404, "Match not found")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateMatch(string matchId)
        {
            await this.mediator.Send(new UpdateLoLGameCommand { MatchId = matchId });

            return this.NoContent();
        }
    }
}
