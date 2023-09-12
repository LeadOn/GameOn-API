// <copyright file="SeasonController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Business.Contracts;
    using YuGames.DTOs;
    using YuGames.Entities;
    using YuGames.WebAPI.Classes;

    /// <summary>
    /// Season Controller.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SeasonController : ControllerBase
    {
        private ISeasonBusiness seasonBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonController"/> class.
        /// </summary>
        /// <param name="seasonBusi">Season business interface (injected).</param>
        public SeasonController(ISeasonBusiness seasonBusi)
        {
            this.seasonBusi = seasonBusi;
        }

        /// <summary>
        /// Get current Season.
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
            var season = await this.seasonBusi.GetCurrent();

            if (season == null)
            {
                return this.NotFound();
            }

            return this.Ok(season);
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
            return this.Ok(await this.seasonBusi.GetAll());
        }
    }
}