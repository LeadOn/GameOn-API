// <copyright file="CoachController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.LeagueOfLegends.Coach.Commands.GenerateLoLCoachReport;
    using GameOn.Application.LeagueOfLegends.Coach.Queries.GetLoLCoachReport;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.External.Llm.Exceptions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// AI Coach Controller.
    /// </summary>
    /// <remarks>
    /// A report is never written on its own: reading is free and open, writing costs a call to the model and
    /// therefore requires an explicit, authenticated request.
    /// </remarks>
    [ApiController]
    [Route("lol/[controller]")]
    public class CoachController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoachController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public CoachController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the AI coach report already written for a player on a game, without generating anything.
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <param name="playerId">GameOn! Player ID.</param>
        /// <returns>200 OK with the report, 404 if none has been generated yet.</returns>
        [HttpGet]
        [Route("{matchId}/player/{playerId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Gets an existing AI coach report.", Description = "Never triggers a generation. A 404 simply means nobody has asked for this analysis yet - the front should offer the button rather than treat it as an error.")]
        [SwaggerResponse(200, "Coach report.", typeof(LoLCoachReportDto))]
        [SwaggerResponse(404, "No coach report generated yet for this match and player.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetReport(string matchId, int playerId)
        {
            var report = await this.mediator.Send(new GetLoLCoachReportQuery { MatchId = matchId, PlayerId = playerId });

            if (report is null)
            {
                return this.NotFound();
            }

            return this.Ok(report);
        }

        /// <summary>
        /// Generates the AI coach report for a player on a game, or returns the one already stored.
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <param name="playerId">GameOn! Player ID.</param>
        /// <param name="force">Regenerate even if a report already exists. Administrators only.</param>
        /// <returns>200 OK with the report.</returns>
        [HttpPost]
        [Route("{matchId}/player/{playerId:int}")]
        [Authorize]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Generates the AI coach report for a player on a game.", Description = "Blocks for as long as the model takes, roughly fifteen seconds - the front should show a spinner. A second call returns the stored report instantly instead of paying for it again.")]
        [SwaggerResponse(200, "Coach report.", typeof(LoLCoachReportDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(404, "Match not found, or this player did not play it.")]
        [SwaggerResponse(429, "The coach is busy or the provider quota is exhausted. Retry shortly.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Generate(string matchId, int playerId, bool force = false)
        {
            LoLCoachReportDto? report;

            // The one place in this codebase that catches a domain exception, and reluctantly: the global
            // exception middleware the conventions assume does not actually exist in Program.cs, so without this
            // a provider quota refusal would reach the caller as a bare 500 - reported as a defect when it is
            // simply "come back in a minute". Remove this the day that middleware is written.
            try
            {
                report = await this.mediator.Send(new GenerateLoLCoachReportCommand
                {
                    MatchId = matchId,
                    PlayerId = playerId,

                    // Regenerating throws away an answer that already exists and pays for it again, so the flag
                    // is honoured only for administrators - any caller may pass it, nobody else gets it.
                    ForceRegenerate = force && this.User.IsInRole("gameon_admin"),
                });
            }
            catch (LlmTransientException)
            {
                this.Response.Headers.RetryAfter = "30";

                return this.StatusCode(
                    StatusCodes.Status429TooManyRequests,
                    new { Error = "Le coach est déjà en train d'analyser une partie, ou le quota du modèle est épuisé. Réessaie dans un instant." });
            }

            if (report is null)
            {
                return this.NotFound();
            }

            return this.Ok(report);
        }
    }
}
