// <copyright file="SoccerFiveController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers
{
    using AutoMapper.Configuration.Annotations;
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
    using GameOn.Application.SoccerFives.Commands.DeleteSoccerFive;
    using GameOn.Application.SoccerFives.Commands.UpdateSoccerFive;
    using GameOn.Application.SoccerFives.Commands.UpdateSoccerFiveSurvey;
    using GameOn.Application.SoccerFives.Commands.VoteSoccerFive;
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
        [SwaggerResponse(201, "Created soccer five.", typeof(CreateSoccerFiveDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Create(CreateSoccerFiveDto five)
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

        /// <summary>
        /// Update soccer five in database.
        /// </summary>
        /// <param name="five">Soccer five.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update Soccer five in database.")]
        [SwaggerResponse(201, "Updated soccer five.", typeof(SoccerFiveDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Update(UpdateSoccerFiveDto five)
        {
            var name = five.Name == null || five.Name == string.Empty ? null : five.Name;
            var description = five.Description == null || five.Description == string.Empty ? null : five.Description;
            var state = five.State == null ? null : five.State;
            var plannedOn = five.PlannedOn == null ? null : five.PlannedOn;

            var currentUser = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (currentUser is null)
            {
                return this.Problem();
            }

            var soccerFiveInDb = await this.mediator.Send(new GetSoccerFiveByIdQuery { SoccerFiveId = five.Id });

            if (soccerFiveInDb is null)
            {
                return this.NotFound();
            }

#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            if (soccerFiveInDb.CreatedBy.Id != currentUser.Id)
            {
                return this.Forbid();
            }
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.

            soccerFiveInDb.Name = name;
            soccerFiveInDb.Description = description;

            if (state != null && (state == SoccerFiveStates.Draft || state == SoccerFiveStates.Planned || state == SoccerFiveStates.Done))
            {
                soccerFiveInDb.State = (int)state;
            }

            soccerFiveInDb.PlannedOn = plannedOn;

            await this.mediator.Send(new UpdateSoccerFiveCommand { Name = soccerFiveInDb.Name, Description = soccerFiveInDb.Description, State = soccerFiveInDb.State, PlannedOn = soccerFiveInDb.PlannedOn, SoccerFiveId = soccerFiveInDb.Id });

            return this.StatusCode(201, await this.mediator.Send(new GetSoccerFiveByIdQuery { SoccerFiveId = soccerFiveInDb.Id }));
        }

        /// <summary>
        /// Update soccer five survey.
        /// </summary>
        /// <param name="id">Soccer five ID.</param>
        /// <param name="survey"><see cref="UpdateSoccerFiveSurvey"/>.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize]
        [Route("{id}/survey")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update five survey.")]
        [SwaggerResponse(201, "Updated soccer five.", typeof(SoccerFiveDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateSurvey(int id, [FromBody] UpdateSoccerFiveSurvey survey)
        {
            var currentUser = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (currentUser is null)
            {
                return this.Problem();
            }

            return this.StatusCode(201, await this.mediator.Send(new UpdateSoccerFiveSurveyCommand { CurrentPlayerId = currentUser.Id, Survey = survey }));
        }

        /// <summary>
        /// Vote soccer five survey.
        /// </summary>
        /// <param name="id">Soccer five ID.</param>
        /// <param name="vote"><see cref="VoteSoccerFiveDto"/>.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize]
        [Route("{id}/survey/vote")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Vote five survey.")]
        [SwaggerResponse(204, "Vote succesfully posted.", typeof(SoccerFiveDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> VoteFive(int id, [FromBody] VoteSoccerFiveDto vote)
        {
            var currentUser = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (currentUser is null)
            {
                return this.Problem();
            }

            vote.PlayerId = currentUser.Id;

            var voteResult = await this.mediator.Send(new VoteSoccerFiveCommand { Vote = vote });

            if (voteResult)
            {
                return this.NoContent();
            }
            else
            {
                return this.Problem();
            }
        }

        /// <summary>
        /// Delete soccer five.
        /// </summary>
        /// <param name="id">Soccer five ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Delete soccer five.")]
        [SwaggerResponse(204, "Successfully deleted.", typeof(SoccerFiveDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (currentUser is null)
            {
                return this.Problem();
            }

            await this.mediator.Send(new DeleteSoccerFiveCommand { SoccerFiveId = id, CurrentPlayerId = currentUser.Id });
            return this.Accepted();
        }
    }
}
