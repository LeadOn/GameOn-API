// <copyright file="MatchController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.Common.Players.Queries.GetConnectedPlayer;
    using GameOn.Application.LeagueOfLegends.Matches.Commands.ImportLoLGames;
    using GameOn.Application.LeagueOfLegends.Matches.Commands.UpdateLoLGame;
    using GameOn.Application.LeagueOfLegends.Matches.Queries.GetGameById;
    using GameOn.Application.LeagueOfLegends.Matches.Queries.GetGameTimelineByMatchId;
    using GameOn.Application.LeagueOfLegends.Matches.Queries.GetLastGamesPlayed;
    using GameOn.Domain;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Match Controller.
    /// </summary>
    [ApiController]
    [Route("lol/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public MatchController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get last matches played by Summoner.
        /// </summary>
        /// <param name="playerId">GameOn! Player ID.</param>
        /// <param name="page">Pagination current page.</param>
        /// <param name="size">Pagination size.</param>
        /// <param name="rankedOnly">Only get ranked only games.</param>
        /// <param name="queues">Filter games by queue IDs, comma-separated (Riot queueId, see LoLQueue).</param>
        /// <returns>200 OK with Player's game list.</returns>
        [HttpGet]
        [Route("player/{playerId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get last games played by summoner.")]
        [SwaggerResponse(200, "Games played.", typeof(List<LoLGame>))]
        [SwaggerResponse(404, "Player not found / no Riot Games PUUID found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLastGamesForUser(int playerId, int? page, int? size, bool rankedOnly = false, string? queues = null)
        {
            var lastGames = await this.mediator.Send(new GetLastGamesPlayedQuery { PlayerId = playerId, Page = page ?? 1, NumberOfResults = size ?? 10, RankedGamesOnly = rankedOnly, QueueIds = ParseQueueIds(queues) });

            if (lastGames is not null)
            {
                return this.Ok(lastGames);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get last matches played.
        /// </summary>
        /// <param name="page">Pagination current page.</param>
        /// <param name="size">Pagination size.</param>
        /// <param name="rankedOnly">Only get ranked only games.</param>
        /// <param name="queues">Filter games by queue IDs, comma-separated (Riot queueId, see LoLQueue).</param>
        /// <returns>200 OK with game list.</returns>
        [HttpGet]
        [Route("last")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get last games played.")]
        [SwaggerResponse(200, "Games played.", typeof(List<LoLGame>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLastGamesPlayed(int? page, int? size, bool rankedOnly = false, string? queues = null)
        {
            var lastGames = await this.mediator.Send(new GetLastGamesPlayedQuery { Page = page ?? 1, NumberOfResults = size ?? 10, RankedGamesOnly = rankedOnly, QueueIds = ParseQueueIds(queues) });

            if (lastGames is not null)
            {
                return this.Ok(lastGames);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get match by ID.
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <returns>200 OK with Match.</returns>
        [HttpGet]
        [Route("{matchId}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get Match by ID.")]
        [SwaggerResponse(200, "Match.", typeof(LoLGame))]
        [SwaggerResponse(404, "Match not found")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(string matchId)
        {
            return this.Ok(await this.mediator.Send(new GetGameByIdQuery { MatchId = matchId }));
        }

        /// <summary>
        /// Get match timeline by ID.
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <returns>200 OK with Match timeline.</returns>
        [HttpGet]
        [Route("{matchId}/timeline")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get Match Timeline by ID.")]
        [SwaggerResponse(200, "Match Timeline.", typeof(List<LoLGameTimelineFrame>))]
        [SwaggerResponse(404, "Match not found")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetGameTimeline(string matchId)
        {
            return this.Ok(await this.mediator.Send(new GetGameTimelineByMatchIdQuery { MatchId = matchId }));
        }

        /// <summary>
        /// Update match by ID.
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <returns>200 OK with Match.</returns>
        [HttpPost]
        [Route("{matchId}/update")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update Match information (retrieve data from Riot Games).")]
        [SwaggerResponse(204, "Success.")]
        [SwaggerResponse(404, "Match not found")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateMatch(string matchId)
        {
            await this.mediator.Send(new UpdateLoLGameCommand { MatchId = matchId });

            return this.NoContent();
        }

        /// <summary>
        /// Import a match by ID (retrieve data from Riot Games and store it in database).
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <param name="executeUpdate">If true, immediately retrieves the match's details from Riot Games after importing it.</param>
        /// <returns>204 No Content.</returns>
        [HttpPost]
        [Authorize]
        [Route("{matchId}/import")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Import a Match by ID (retrieve data from Riot Games).")]
        [SwaggerResponse(204, "Success.")]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(404, "Connected player not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> ImportMatch(string matchId, bool executeUpdate = true)
        {
            var connectedPlayer = await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (connectedPlayer is null)
            {
                return this.NotFound();
            }

            await this.mediator.Send(new ImportLoLGamesCommand { MatchIDs = new List<string> { matchId }, Player = connectedPlayer, ExecuteUpdate = executeUpdate });

            return this.NoContent();
        }

        /// <summary>
        /// Parses a comma-separated list of queue IDs (e.g. "420,440") into a list of ints.
        /// </summary>
        /// <param name="queues">Comma-separated queue IDs.</param>
        /// <returns>Parsed list, or null if the input is empty.</returns>
        private static List<int>? ParseQueueIds(string? queues)
        {
            if (string.IsNullOrWhiteSpace(queues))
            {
                return null;
            }

            return queues
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToList();
        }
    }
}
