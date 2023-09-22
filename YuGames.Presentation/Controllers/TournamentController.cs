// <copyright file="TournamentController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.Tournaments.Commands.CreateTournament;
    using YuGames.Application.Tournaments.Commands.UpdateTournament;
    using YuGames.Application.Tournaments.Queries.CheckTournamentSubscription;
    using YuGames.Application.Tournaments.Queries.GetAllTournaments;
    using YuGames.Application.Tournaments.Queries.GetTournamentById;
    using YuGames.Common.DTOs;
    using YuGames.Domain;
    using YuGames.Presentation.Classes;

    /// <summary>
    /// Tournament Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TournamentController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public TournamentController(ISender mediator)
        {
            this.mediator = mediator;
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
            return this.Ok(await this.mediator.Send(new GetAllTournamentsQuery()));
        }

        /// <summary>
        /// Get tournament by ID in database.
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
            return this.Ok(await this.mediator.Send(new GetTournamentByIdQuery { TournamentId = id }));
        }

        /// <summary>
        /// Check player subscription to a tournament in database.
        /// </summary>
        /// <param name="id">Tournament ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Authorize]
        [Route("{id:int}/subscription")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Check if player is subscribed to a tournament.")]
        [SwaggerResponse(204, "Player is subscribed.")]
        [SwaggerResponse(404, "Player not subscribed / tournament not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> CheckSubscription(int id)
        {
            var playerSubscription = await this.mediator.Send(new CheckTournamentSubscriptionQuery { TournamentId = id, ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (playerSubscription)
            {
                return this.NoContent();
            }

            return this.NotFound();
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
            var newTournament = new Tournament
            {
                Description = tournament.Description,
                Name = tournament.Name,
                State = tournament.State,
                LogoUrl = tournament.LogoUrl,
                PlannedFrom = tournament.PlannedFrom,
                PlannedTo = tournament.PlannedTo,
            };

            return this.StatusCode(201, await this.mediator.Send(new CreateTournamentCommand { Tournament = newTournament }));
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
            return this.Ok(await this.mediator.Send(new UpdateTournamentCommand { TournamentId = id, TournamentDto = tournament }));
        }
    }
}
