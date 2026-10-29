// <copyright file="ValorantHomeController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.Valorant
{
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Home.Queries.GetValHomeStats;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.Valorant;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Valorant Home Controller. Powers the Valorant home page.
    /// </summary>
    [ApiController]
    [Route("val/home")]
    public class ValorantHomeController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantHomeController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public ValorantHomeController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get the aggregated recap powering the Valorant home page.
        /// </summary>
        /// <param name="includeSmurfs">If false, smurf accounts are left out of every block of the recap. Defaults to true.</param>
        /// <param name="includeOutOfCrew">If true, accounts outside the crew count towards every block of the recap too. Defaults to false.</param>
        /// <param name="window">Window of the weekly activity and the fact of the week: CalendarWeek (default) or Last7Days. The crew records always cover the rolling month.</param>
        /// <returns>200 OK with the home page recap.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the Valorant home page recap.")]
        [SwaggerResponse(200, "Home page recap.", typeof(ValHomeStatsDto))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetHomeStats(bool includeSmurfs = true, bool includeOutOfCrew = false, LoLHomeWindow window = LoLHomeWindow.CalendarWeek)
        {
            return this.Ok(await this.mediator.Send(new GetValHomeStatsQuery
            {
                IncludeSmurfs = includeSmurfs,
                IncludeOutOfCrew = includeOutOfCrew,
                Window = window,
            }));
        }
    }
}
