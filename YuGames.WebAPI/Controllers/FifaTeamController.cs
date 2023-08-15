// <copyright file="FifaTeamController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Business.Contracts;
    using YuGames.Entities;

    /// <summary>
    /// FifaTeam Controller.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class FifaTeamController : ControllerBase
    {
        private IFifaTeamBusiness fifaTeamBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="FifaTeamController"/> class.
        /// </summary>
        /// <param name="fifaTeamBusi">FifaTeam business interface (injected).</param>
        public FifaTeamController(IFifaTeamBusiness fifaTeamBusi)
        {
            this.fifaTeamBusi = fifaTeamBusi;
        }

        /// <summary>
        /// Get all FIFA Teams in database.
        /// </summary>
        /// <returns>200 OK with Fifa Team list.</returns>
        [HttpGet]
        [Route("all")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all FIFA Teams in database.")]
        [SwaggerResponse(200, "FIFA Teams in database.", typeof(List<FifaTeam>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.fifaTeamBusi.GetAll());
        }
    }
}