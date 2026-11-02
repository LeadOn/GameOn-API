// <copyright file="ValorantAdminController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.Valorant
{
    using System;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Background.Commands.EnqueueValorantBackgroundTasks;
    using GameOn.Application.Valorant.Background.Commands.RecomputeValGameStats;
    using GameOn.Application.Valorant.Background.Queries.GetValorantBackgroundQueueStatus;
    using GameOn.Common.DTOs.Valorant;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Valorant data maintenance: backfill, reimport, daily extras and stat recompute. Every HenrikDev call these
    /// make is low priority and runs in the background, one task at a time.
    /// </summary>
    [ApiController]
    [Authorize(Roles = "gameon_admin")]
    [Route("val/admin")]
    public class ValorantAdminController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantAdminController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public ValorantAdminController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Queue the import of the history HenrikDev stored (MMR history, then every stored match missing from
        /// the database, 1 HenrikDev call per match).
        /// </summary>
        /// <param name="playerId">Player, omitted for every Valorant account.</param>
        /// <returns>202 Accepted with the state of the queue.</returns>
        [HttpPost]
        [Route("backfill")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Queue a Valorant history backfill.")]
        [SwaggerResponse(202, "Queued.", typeof(ValBackgroundQueueStatusDto))]
        public async Task<IActionResult> Backfill(int? playerId)
        {
            return this.Accepted(await this.mediator.Send(new EnqueueValorantBackgroundTasksCommand { Kind = ValBackgroundTaskKind.Backfill, PlayerId = playerId }));
        }

        /// <summary>
        /// Queue the reimport of matches already in database (1 HenrikDev call per match), to fill the columns
        /// added since they were imported.
        /// </summary>
        /// <param name="playerId">Player whose matches are re-imported, omitted for every match.</param>
        /// <param name="since">Oldest match start re-imported (UTC), omitted for no bound.</param>
        /// <returns>202 Accepted with the state of the queue.</returns>
        [HttpPost]
        [Route("reimport")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Queue a Valorant match reimport.")]
        [SwaggerResponse(202, "Queued.", typeof(ValBackgroundQueueStatusDto))]
        public async Task<IActionResult> Reimport(int? playerId, DateTime? since)
        {
            return this.Accepted(await this.mediator.Send(new EnqueueValorantBackgroundTasksCommand { Kind = ValBackgroundTaskKind.Reimport, PlayerId = playerId, Since = since }));
        }

        /// <summary>
        /// Queue the refresh of accolades, agent mastery and Premier team (3 HenrikDev calls per account). Also
        /// runs on its own once a day.
        /// </summary>
        /// <param name="playerId">Player, omitted for every Valorant account.</param>
        /// <returns>202 Accepted with the state of the queue.</returns>
        [HttpPost]
        [Route("extras")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Queue a Valorant accolades, mastery and Premier refresh.")]
        [SwaggerResponse(202, "Queued.", typeof(ValBackgroundQueueStatusDto))]
        public async Task<IActionResult> Extras(int? playerId)
        {
            return this.Accepted(await this.mediator.Send(new EnqueueValorantBackgroundTasksCommand { Kind = ValBackgroundTaskKind.Extras, PlayerId = playerId }));
        }

        /// <summary>
        /// Recompute every derived figure of the stored matches (sides, zones, kill distances, stats, MVP and
        /// ACE) from the database alone. No HenrikDev call.
        /// </summary>
        /// <param name="since">Oldest match start recomputed (UTC), omitted for every match.</param>
        /// <returns>200 OK with the number of matches recomputed.</returns>
        [HttpPost]
        [Route("recompute-stats")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Recompute Valorant match stats from the database.")]
        [SwaggerResponse(200, "Number of matches recomputed.", typeof(int))]
        public async Task<IActionResult> RecomputeStats(DateTime? since)
        {
            return this.Ok(await this.mediator.Send(new RecomputeValGameStatsCommand { Since = since }));
        }

        /// <summary>
        /// Get the state of the Valorant background queue: running task, waiting tasks, last results.
        /// </summary>
        /// <returns>200 OK with the state of the queue.</returns>
        [HttpGet]
        [Route("queue")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the Valorant background queue.")]
        [SwaggerResponse(200, "State of the queue.", typeof(ValBackgroundQueueStatusDto))]
        public async Task<IActionResult> GetQueue()
        {
            return this.Ok(await this.mediator.Send(new GetValorantBackgroundQueueStatusQuery()));
        }
    }
}
