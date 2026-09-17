// <copyright file="CoachController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.LeagueOfLegends.Coach.Commands.EnqueueLoLCoachReport;
    using GameOn.Application.LeagueOfLegends.Coach.Queries.GetLoLCoachQueueStatus;
    using GameOn.Application.LeagueOfLegends.Coach.Queries.GetLoLCoachReport;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// AI Coach Controller.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A report is never written on its own: reading is free and open, writing costs a call to the model and
    /// therefore requires an explicit, authenticated request.
    /// </para>
    /// <para>
    /// Neither route ever calls the model. Asking for an analysis puts it in a line that a single background
    /// consumer works through, and both routes answer 202 with the place in that line while it waits. Holding
    /// the connection open instead - which is what this controller used to do - only ever worked for one
    /// player at a time: a generation takes about fifty seconds against a provider that allows five calls a
    /// minute, so a second simultaneous click could do nothing but fail.
    /// </para>
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
        /// Gets the AI coach report already written for a player on a game, without asking for anything.
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <param name="playerId">GameOn! Player ID.</param>
        /// <returns>200 OK with the report, 202 with its place in the line, or 404 if nobody has asked.</returns>
        [HttpGet]
        [Route("{matchId}/player/{playerId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Gets an existing AI coach report, or its place in the queue.", Description = "Never queues anything. Poll this while a 202 is coming back. A 404 means nobody has asked for this analysis yet - the front should offer the button rather than treat it as an error.")]
        [SwaggerResponse(200, "Coach report.", typeof(LoLCoachReportDto))]
        [SwaggerResponse(202, "Analysis requested and waiting.", typeof(LoLCoachQueueStatusDto))]
        [SwaggerResponse(404, "No coach report generated yet for this match and player.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetReport(string matchId, int playerId)
        {
            var report = await this.mediator.Send(new GetLoLCoachReportQuery { MatchId = matchId, PlayerId = playerId });

            if (report is not null)
            {
                return this.Ok(report);
            }

            var queued = await this.mediator.Send(new GetLoLCoachQueueStatusQuery { MatchId = matchId, PlayerId = playerId });

            if (queued is not null)
            {
                return this.Accepted(queued);
            }

            return this.NotFound();
        }

        /// <summary>
        /// Asks for the AI coach report of a player on a game, or returns the one already stored.
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <param name="playerId">GameOn! Player ID.</param>
        /// <param name="force">Regenerate even if a report already exists. Administrators only.</param>
        /// <returns>200 OK with the stored report, or 202 with its place in the line.</returns>
        [HttpPost]
        [Route("{matchId}/player/{playerId:int}")]
        [Authorize]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Asks for the AI coach report of a player on a game.", Description = "Returns immediately. A stored report comes back as 200; anything else is queued and comes back as 202 with a position and an estimated wait, which the front should poll the GET route for. Pressing twice does not buy two slots.")]
        [SwaggerResponse(200, "Coach report, already stored.", typeof(LoLCoachReportDto))]
        [SwaggerResponse(202, "Analysis queued.", typeof(LoLCoachQueueStatusDto))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(404, "Match not found, or this player did not play it.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> Generate(string matchId, int playerId, bool force = false)
        {
            // Regenerating throws away an answer that already exists and pays for it again, so the flag is
            // honoured only for administrators - any caller may pass it, nobody else gets it.
            var forceRegenerate = force && this.User.IsInRole("gameon_admin");

            if (!forceRegenerate)
            {
                // A stored report is served without ever touching the line: the cache is what guarantees the
                // same analysis is never paid for twice.
                var existing = await this.mediator.Send(new GetLoLCoachReportQuery { MatchId = matchId, PlayerId = playerId });

                if (existing is not null)
                {
                    return this.Ok(existing);
                }
            }

            var queued = await this.mediator.Send(new EnqueueLoLCoachReportCommand
            {
                MatchId = matchId,
                PlayerId = playerId,
                ForceRegenerate = forceRegenerate,
            });

            if (queued is null)
            {
                return this.NotFound();
            }

            return this.Accepted(queued);
        }
    }
}
