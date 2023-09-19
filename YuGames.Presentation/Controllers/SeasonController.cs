// <copyright file="SeasonController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.FifaTeams.Queries.GetAllFifaTeams;
    using YuGames.Application.Seasons.Queries.GetAllSeasons;
    using YuGames.Application.Seasons.Queries.GetCurrentSeason;
    using YuGames.Domain;

    /// <summary>
    /// Season Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SeasonController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public SeasonController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all seasons.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all seasons.")]
        [SwaggerResponse(200, "List of Season.", typeof(List<Season>))]
        [SwaggerResponse(500, "Something wrong happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.mediator.Send(new GetAllSeasonsQuery()));
        }

        /// <summary>
        /// Get current season.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("current")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get current season.")]
        [SwaggerResponse(200, "Current Season.", typeof(Season))]
        [SwaggerResponse(404, "Current Season not found.")]
        [SwaggerResponse(500, "Something wrong happened.")]
        public async Task<IActionResult> GetCurrentSeason()
        {
            var season = await this.mediator.Send(new GetCurrentSeasonQuery());

            if (season is null)
            {
                return this.NotFound();
            }

            return this.Ok(season);
        }
    }
}
