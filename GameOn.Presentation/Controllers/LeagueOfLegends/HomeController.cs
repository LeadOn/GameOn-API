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
        /// <returns>200 OK with the home page recap.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the League of Legends home page recap.")]
        [SwaggerResponse(200, "Home page recap.", typeof(LoLHomeStatsDto))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetHomeStats()
        {
            return this.Ok(await this.mediator.Send(new GetLoLHomeStatsQuery()));
        }
    }
}
