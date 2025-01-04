// <copyright file="SummonerController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.Common.Players.Queries.GetConnectedPlayer;
    using GameOn.Application.Common.Players.Queries.GetPlayerById;
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummoner;
    using GameOn.Application.LeagueOfLegends.Summoners.Queries.GetAllLeaguePlayers;
    using GameOn.Application.LeagueOfLegends.Summoners.Queries.GetLeaguePlayerById;
    using GameOn.Application.LeagueOfLegends.Summoners.Queries.GetSummonerRankHistory;
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Summoner Controller.
    /// </summary>
    [ApiController]
    [Route("lol/[controller]")]
    public class SummonerController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SummonerController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public SummonerController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all league of legends players in database.
        /// </summary>
        /// <param name="archived">If true, get archived players.</param>
        /// <returns>200 OK with Player list.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all League of Legends players in database.")]
        [SwaggerResponse(200, "Players in database.", typeof(List<Player>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll(bool? archived)
        {
            return this.Ok(await this.mediator.Send(new GetAllLeaguePlayersQuery { Archived = archived ?? false }));
        }

        /// <summary>
        /// Get a summoner by its ID.
        /// </summary>
        /// <param name="id">Summoner ID.</param>
        /// <returns>200 OK with Player if found, 404 if not found.</returns>
        [HttpGet]
        [Route("{id:int}")]
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
        /// Get summoner rank history.
        /// </summary>
        /// <param name="id">Summoner ID.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("{id:int}/rank")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get summoner League of Legends history.")]
        [SwaggerResponse(200, "List of rank history.", typeof(List<LeagueOfLegendsRankHistory>))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetRankHistory(int id, int? limit)
        {
#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return this.Ok(await this.mediator.Send(new GetSummonerRankHistoryQuery { PlayerId = id, Limit = limit }));
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.
        }

        /// <summary>
        /// Refresh summoner by ID.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update user League of Legends summoner.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RefreshById(int id)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = id });

#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return this.Ok(await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = playerInDb }));
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.
        }

        /// <summary>
        /// Update summoner of connected player.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize]
        [Route("me")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update current user League of Legends summoner.")]
        [SwaggerResponse(200, "Updated user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> RefreshConnected()
        {
            var playerInDb = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return this.Ok(await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = playerInDb }));
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.
        }
    }
}
