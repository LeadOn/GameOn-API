// <copyright file="HomeController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.LeagueOfLegends.Home.Queries.GetLoLHomeStats;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// League of Legends Home Controller. Powers the v2 site's League of Legends home page.
    /// </summary>
    [ApiController]
    [Route("lol/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public HomeController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get the aggregated recap powering the League of Legends home page.
        /// </summary>
        /// <param name="includeSmurfs">If false, smurf accounts are left out of every block of the recap. Defaults to true: a smurf's games were played, so they weigh on the week like any other.</param>
        /// <param name="includeOutOfCrew">If true, accounts outside the crew count towards every block of the recap too. Defaults to false: this page is the crew's dashboard.</param>
        /// <param name="window">Window of the weekly activity and the fact of the week: CalendarWeek (default, Monday 00:00 Europe/Paris to now, against the previous full week) or Last7Days (six days ago 00:00 Europe/Paris to now, against the seven days before). The crew records always cover the rolling month.</param>
        /// <returns>200 OK with the home page recap.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the League of Legends home page recap.")]
        [SwaggerResponse(200, "Home page recap.", typeof(LoLHomeStatsDto))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetHomeStats(bool includeSmurfs = true, bool includeOutOfCrew = false, LoLHomeWindow window = LoLHomeWindow.CalendarWeek)
        {
            return this.Ok(await this.mediator.Send(new GetLoLHomeStatsQuery
            {
                IncludeSmurfs = includeSmurfs,
                IncludeOutOfCrew = includeOutOfCrew,
                Window = window,
            }));
        }
    }
}
