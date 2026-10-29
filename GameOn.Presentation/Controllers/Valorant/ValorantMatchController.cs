// <copyright file="ValorantMatchController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.Valorant
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Commands.UpdateValGame;
    using GameOn.Application.Valorant.Matches.Queries.GetLastValGamesPlayed;
    using GameOn.Application.Valorant.Matches.Queries.GetValGameById;
    using GameOn.Application.Valorant.Matches.Queries.GetValGameTimelineByMatchId;
    using GameOn.Common.DTOs.Common;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Valorant Match Controller.
    /// </summary>
    [ApiController]
    [Route("val/match")]
    public class ValorantMatchController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValorantMatchController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public ValorantMatchController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the last Valorant matches played by a specific player.
        /// </summary>
        /// <param name="playerId">GameOn! Player ID.</param>
        /// <param name="page">Pagination current page (1-based).</param>
        /// <param name="size">Pagination size.</param>
        /// <param name="rankedOnly">Only return competitive (ranked) matches.</param>
        /// <param name="queues">Comma-separated queue IDs (e.g. "competitive,unrated,swiftplay,custom").</param>
        /// <param name="agentName">Only return matches where the player played this agent (e.g. "Jett", "Raze").</param>
        /// <param name="startDate">Only return matches starting on or after this date (inclusive).</param>
        /// <param name="endDate">Only return matches starting on or before this date (inclusive).</param>
        /// <param name="refresh">If true, fetches the latest 10 matches from HenrikDev before querying the database.</param>
        /// <returns>200 OK with paginated Valorant match list.</returns>
        [HttpGet]
        [Route("player/{playerId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get last Valorant matches played by a player.")]
        [SwaggerResponse(200, "Valorant matches played.", typeof(ListResultDto<ValGame>))]
        [SwaggerResponse(404, "Player not found or has no Valorant PUUID.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLastGamesForUser(
            int playerId,
            int? page = null,
            int? size = null,
            bool rankedOnly = false,
            string? queues = null,
            string? agentName = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool refresh = false)
        {
            var lastGames = await this.mediator.Send(new GetLastValGamesPlayedQuery
            {
                PlayerId = playerId,
                Page = page ?? 1,
                NumberOfResults = size ?? 10,
                RankedGamesOnly = rankedOnly,
                Queues = ParseQueues(queues),
                AgentName = agentName,
                StartDate = startDate,
                EndDate = endDate,
                SyncFromApi = refresh,
            });

            if (lastGames is null)
            {
                return this.NotFound();
            }

            return this.Ok(lastGames);
        }

        /// <summary>
        /// Gets the last Valorant matches played by the crew.
        /// </summary>
        /// <param name="page">Pagination current page (1-based).</param>
        /// <param name="size">Pagination size.</param>
        /// <param name="rankedOnly">Only return competitive (ranked) matches.</param>
        /// <param name="queues">Comma-separated queue IDs (e.g. "competitive,unrated,swiftplay,custom").</param>
        /// <param name="startDate">Only return matches starting on or after this date (inclusive).</param>
        /// <param name="endDate">Only return matches starting on or before this date (inclusive).</param>
        /// <param name="includeSmurfs">If false, matches played by smurf accounts alone are excluded.</param>
        /// <param name="includeOutOfCrew">If true, matches played by accounts outside the crew alone are included.</param>
        /// <returns>200 OK with paginated Valorant match list.</returns>
        [HttpGet]
        [Route("last")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get last Valorant matches played by the crew.")]
        [SwaggerResponse(200, "Valorant matches played.", typeof(ListResultDto<ValGame>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLastGamesPlayed(
            int? page = null,
            int? size = null,
            bool rankedOnly = false,
            string? queues = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool includeSmurfs = true,
            bool includeOutOfCrew = false)
        {
            var lastGames = await this.mediator.Send(new GetLastValGamesPlayedQuery
            {
                Page = page ?? 1,
                NumberOfResults = size ?? 10,
                RankedGamesOnly = rankedOnly,
                Queues = ParseQueues(queues),
                StartDate = startDate,
                EndDate = endDate,
                IncludeSmurfs = includeSmurfs,
                IncludeOutOfCrew = includeOutOfCrew,
            });

            return this.Ok(lastGames);
        }

        /// <summary>
        /// Gets a single Valorant match by its UUID.
        /// </summary>
        /// <param name="matchId">Valorant match UUID.</param>
        /// <returns>200 OK with Valorant match.</returns>
        [HttpGet]
        [Route("{matchId}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get Valorant match by ID.")]
        [SwaggerResponse(200, "Valorant match.", typeof(ValGame))]
        [SwaggerResponse(404, "Match not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(string matchId)
        {
            var game = await this.mediator.Send(new GetValGameByIdQuery { MatchId = matchId });

            if (game is null)
            {
                return this.NotFound();
            }

            return this.Ok(game);
        }

        /// <summary>
        /// Gets the round-by-round breakdown and chronological kill events for a Valorant match.
        /// </summary>
        /// <param name="matchId">Valorant match UUID.</param>
        /// <returns>200 OK with rounds and kill events.</returns>
        [HttpGet]
        [Route("{matchId}/rounds")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get Valorant match rounds and kills by ID.")]
        [SwaggerResponse(200, "Valorant match rounds and kills.", typeof(ValGameTimelineDto))]
        [SwaggerResponse(404, "Match not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetRoundsById(string matchId)
        {
            var timeline = await this.mediator.Send(new GetValGameTimelineByMatchIdQuery { MatchId = matchId });

            if (timeline is null)
            {
                return this.NotFound();
            }

            return this.Ok(timeline);
        }

        /// <summary>
        /// Fetches or refreshes a single Valorant match by ID from HenrikDev and updates it in database.
        /// </summary>
        /// <param name="matchId">Valorant match UUID.</param>
        /// <param name="region">Optional Valorant region (defaults to the stored match region or "eu").</param>
        /// <returns>200 OK with the updated Valorant match.</returns>
        [HttpPost]
        [Route("{matchId}/update")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Fetch or refresh a Valorant match by ID from HenrikDev.")]
        [SwaggerResponse(200, "Updated Valorant match.", typeof(ValGame))]
        [SwaggerResponse(404, "Match not found on HenrikDev.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateMatch(string matchId, string? region = null)
        {
            var updated = await this.mediator.Send(new UpdateValGameCommand { MatchId = matchId, Region = region });

            if (updated is null)
            {
                return this.NotFound();
            }

            return this.Ok(updated);
        }

        private static List<string>? ParseQueues(string? queues)
        {
            if (string.IsNullOrWhiteSpace(queues))
            {
                return null;
            }

            return queues
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
        }
    }
}
