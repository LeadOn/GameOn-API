// <copyright file="ValorantPremierController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.Valorant
{
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Premier.Queries.GetValorantPremierTeam;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Valorant Premier teams of our players.
    /// </summary>
    [ApiController]
    [Route("val/premier")]
    public class ValorantPremierController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantPremierController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public ValorantPremierController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get a Premier team by its roster ID (<c>valorantPremierTeamId</c> on a player). Refreshed once a day.
        /// </summary>
        /// <param name="teamId">Premier roster ID.</param>
        /// <returns>200 OK with the team, 404 when unknown.</returns>
        [HttpGet]
        [Route("{teamId}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a Valorant Premier team.")]
        [SwaggerResponse(200, "The team.", typeof(ValorantPremierTeam))]
        [SwaggerResponse(404, "Unknown team.")]
        public async Task<IActionResult> GetTeam(string teamId)
        {
            var team = await this.mediator.Send(new GetValorantPremierTeamQuery { TeamId = teamId });
            return team is null ? this.NotFound() : this.Ok(team);
        }
    }
}
