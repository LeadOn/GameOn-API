// <copyright file="SoccerFiveController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers
{
    using GameOn.Application.FifaGamePlayed.Commands.CreateFifaGamePlayed;
    using GameOn.Application.FifaGamePlayed.Commands.DeleteFifaGamePlayed;
    using GameOn.Application.FifaGamePlayed.Commands.UpdateFifaGamePlayed;
    using GameOn.Application.FifaGamePlayed.Queries.GetFifaGamePlayedById;
    using GameOn.Application.FifaGamePlayed.Queries.GetFifaGamePlayedByTournamentId;
    using GameOn.Application.FifaGamePlayed.Queries.GetLastFifaGamesPlayed;
    using GameOn.Application.FifaGamePlayed.Queries.GetLastFifaGamesPlayedByPlayerId;
    using GameOn.Application.FifaGamePlayed.Queries.SearchFifaGamesPlayed;
    using GameOn.Application.Players.Queries.GetConnectedPlayer;
    using GameOn.Application.SoccerFives.Commands.CreateSoccerFive;
    using GameOn.Application.TournamentPlayers.Queries.GetSoccerFiveById;
    using GameOn.Application.TournamentPlayers.Queries.GetSoccerFives;
    using GameOn.Application.Tournaments.Commands.CreateTournament;
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// SoccerFive Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SoccerFiveController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoccerFiveController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public SoccerFiveController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets all soccer fivesgames.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all soccer fives created.")]
        [SwaggerResponse(200, "List of soccer five played.", typeof(List<SoccerFive>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.mediator.Send(new GetSoccerFivesQuery()));
        }

        /// <summary>
        /// Gets soccer five by ID.
        /// </summary>
        /// <param name="id">Soccer five ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all soccer fives created.")]
        [SwaggerResponse(200, "List of soccer five played.", typeof(List<SoccerFive>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Get(int id)
        {
            var five = await this.mediator.Send(new GetSoccerFiveByIdQuery { SoccerFiveId = id });

            if (five is null)
            {
                return this.NotFound();
            }

            return this.Ok(five);
        }

        /// <summary>
        /// Create soccer five in database.
        /// </summary>
        /// <param name="five">Soccer five.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create Soccer five in database.")]
        [SwaggerResponse(201, "Created soccer five.", typeof(SoccerFiveDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Create(SoccerFiveDto five)
        {
            var name = five.Name == null || five.Name == string.Empty ? null : five.Name;
            var description = five.Description == null || five.Description == string.Empty ? null : five.Description;
            var plannedOn = five.PlannedOn == null ? null : five.PlannedOn;

            var currentUser = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (currentUser is null)
            {
                return this.Problem();
            }

            var newFive = new SoccerFive
            {
                Name = name,
                Description = description,
                PlannedOn = plannedOn,
                CreatedById = currentUser.Id,
            };

            return this.StatusCode(201, await this.mediator.Send(new CreateSoccerFiveCommand { SoccerFive = newFive }));
        }
    }
}
