// <copyright file="QueueController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.LeagueOfLegends.Queues.Commands.SyncQueues;
    using GameOn.Application.LeagueOfLegends.Queues.Queries.GetAllQueues;
    using GameOn.Application.LeagueOfLegends.Queues.Queries.GetPlayerQueues;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Queue Controller.
    /// </summary>
    [ApiController]
    [Route("lol/[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public QueueController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Sync queue types from Riot Games.
        /// </summary>
        /// <returns>204 No Content.</returns>
        [HttpPost]
        [Authorize(Roles = "gameon_admin")]
        [Route("sync")]
        [SwaggerOperation(Summary = "Sync League of Legends queue types from Riot Games.")]
        [SwaggerResponse(204, "Success.")]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> SyncQueues()
        {
            await this.mediator.Send(new SyncQueuesCommand());

            return this.NoContent();
        }

        /// <summary>
        /// Get all League of Legends queues in database.
        /// </summary>
        /// <returns>200 OK with queue list.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all League of Legends queues in database.")]
        [SwaggerResponse(200, "Queues in database.", typeof(List<LoLQueue>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.mediator.Send(new GetAllQueuesQuery()));
        }

        /// <summary>
        /// Get the queues a given player has already played on.
        /// </summary>
        /// <param name="playerId">GameOn! Player ID.</param>
        /// <returns>200 OK with queue list.</returns>
        [HttpGet]
        [Route("player/{playerId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the League of Legends queues a player has already played on.")]
        [SwaggerResponse(200, "Queues played by the player.", typeof(List<LoLQueue>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetPlayerQueues(int playerId)
        {
            return this.Ok(await this.mediator.Send(new GetPlayerQueuesQuery { PlayerId = playerId }));
        }
    }
}
