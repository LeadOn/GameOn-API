// <copyright file="FifaTeamController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.FifaTeams.Queries.GetAllFifaTeams;
    using YuGames.Domain;

    /// <summary>
    /// FifaTeam Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class FifaTeamController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FifaTeamController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public FifaTeamController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all FIFA Teams in database.
        /// </summary>
        /// <returns>200 OK with Fifa Team list.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all FIFA Teams in database.")]
        [SwaggerResponse(200, "FIFA Teams in database.", typeof(List<FifaTeam>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.mediator.Send(new GetAllFifaTeamsQuery()));
        }
    }
}
