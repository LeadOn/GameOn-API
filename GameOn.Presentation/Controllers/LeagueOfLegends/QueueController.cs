// <copyright file="QueueController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.LeagueOfLegends.Queues.Commands.SyncQueues;
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
    }
}
