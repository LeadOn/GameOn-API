// <copyright file="TournamentController.cs" company="LeadOn's Corp'">
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
    /// Tournament Controller.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TournamentController : ControllerBase
    {
        private ITournamentBusiness tournamentBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentController"/> class.
        /// </summary>
        /// <param name="tournamentBusi">Tournament business, injected.</param>
        public TournamentController(ITournamentBusiness tournamentBusi)
        {
            this.tournamentBusi = tournamentBusi;
        }

        /// <summary>
        /// Create tournament in database.
        /// </summary>
        /// <param name="tournament">Tournament DTO.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize(Roles = "yugames_admin")]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create tournament in database.")]
        [SwaggerResponse(201, "Created tournament.", typeof(Tournament))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Create(TournamentDto tournament)
        {
            return this.StatusCode(201, await this.tournamentBusi.Create(tournament));
        }

        /// <summary>
        /// Update tournament in database.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <param name="tournament">Updated tournament.</param>
        /// <returns>IActionResult Object.</returns>
        [HttpPatch]
        [Authorize(Roles = "yugames_admin")]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create tournament in database.")]
        [SwaggerResponse(201, "Created tournament.", typeof(Tournament))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(403, "Not enough roles.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Update(int id, [FromBody]TournamentDto tournament)
        {
            return this.StatusCode(200, await this.tournamentBusi.UpdateTournament(id, tournament));
        }

        /// <summary>
        /// Get all tournaments in database.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all tournaments in database.")]
        [SwaggerResponse(200, "Tournaments.", typeof(List<Tournament>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.tournamentBusi.GetAll());
        }

        /// <summary>
        /// Get all tournaments in database.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get tournament by ID in database.")]
        [SwaggerResponse(200, "Tournament.", typeof(TournamentDto))]
        [SwaggerResponse(404, "Tournament not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int id)
        {
            return this.Ok(await this.tournamentBusi.GetById(id));
        }
    }
}