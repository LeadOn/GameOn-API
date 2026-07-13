// <copyright file="StatsController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.LeagueOfLegends.Stats.Queries.GetLoLGlobalStats;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// League of Legends Stats Controller.
    /// </summary>
    [ApiController]
    [Route("lol/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public StatsController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get global League of Legends fun stats, all players included.
        /// </summary>
        /// <param name="rankedOnly">Only include ranked games (Solo/Duo and Flex).</param>
        /// <param name="queue">Queue restriction: All, Solo or Flex. Solo or Flex implies ranked games only.</param>
        /// <param name="period">Rolling time window: AllTime, Week, Month, ThreeMonths or SixMonths.</param>
        /// <returns>200 OK with global stats recap.</returns>
        [HttpGet]
        [Route("global")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get global League of Legends fun stats, all players included.")]
        [SwaggerResponse(200, "Global fun stats recap.", typeof(LoLGlobalStatsDto))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetGlobalStats(bool rankedOnly = false, LoLQueueFilter queue = LoLQueueFilter.All, LoLStatsPeriod period = LoLStatsPeriod.AllTime)
        {
            return this.Ok(await this.mediator.Send(new GetLoLGlobalStatsQuery { RankedOnly = rankedOnly, Queue = queue, Period = period }));
        }
    }
}
