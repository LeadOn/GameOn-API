// <copyright file="ValorantStatsController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.Valorant
{
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Stats.Queries.GetValGlobalStats;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.Valorant;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Valorant Stats Controller.
    /// </summary>
    [ApiController]
    [Route("val/stats")]
    public class ValorantStatsController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantStatsController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public ValorantStatsController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get the crew's Valorant fun stats and most played agents and maps.
        /// </summary>
        /// <param name="rankedOnly">Only include competitive games.</param>
        /// <param name="period">Rolling time window: AllTime, Week, Month, ThreeMonths or SixMonths.</param>
        /// <param name="includeSmurfs">If false, smurf accounts are left out of the records entirely. Defaults to true.</param>
        /// <param name="includeOutOfCrew">If true, accounts outside the crew take part in the records too. Defaults to false.</param>
        /// <returns>200 OK with the global stats recap.</returns>
        [HttpGet]
        [Route("global")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the crew's Valorant fun stats.")]
        [SwaggerResponse(200, "Global fun stats recap.", typeof(ValGlobalStatsDto))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetGlobalStats(bool rankedOnly = false, LoLStatsPeriod period = LoLStatsPeriod.AllTime, bool includeSmurfs = true, bool includeOutOfCrew = false)
        {
            return this.Ok(await this.mediator.Send(new GetValGlobalStatsQuery
            {
                RankedOnly = rankedOnly,
                Period = period,
                IncludeSmurfs = includeSmurfs,
                IncludeOutOfCrew = includeOutOfCrew,
            }));
        }
    }
}
