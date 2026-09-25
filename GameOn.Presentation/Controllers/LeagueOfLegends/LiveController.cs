// <copyright file="LiveController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.LeagueOfLegends
{
    using GameOn.Application.LeagueOfLegends.Live.Queries.GetLoLLiveGames;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// League of Legends Live Controller. Who is in a game right now.
    /// </summary>
    [ApiController]
    [Route("lol/[controller]")]
    public class LiveController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public LiveController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get the tracked accounts currently in a game, one entry per account. Answers come from Riot's
        /// spectator-v5 through a server-side cache: Riot is asked about each account at most once a minute,
        /// whatever the traffic, so an entry can lag reality by up to a minute.
        /// </summary>
        /// <param name="includeSmurfs">If false, smurf accounts are left out. Defaults to true, like lol/Home.</param>
        /// <param name="includeOutOfCrew">If true, accounts outside the crew are looked up too. Defaults to false, like lol/Home.</param>
        /// <returns>200 OK with the accounts in game, empty when nobody plays.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the tracked accounts currently in a game.", Description = "One entry per account, several accounts in the same game share the same gameId. Served from a server-side cache refreshed at most once a minute per account (see retrievedOn).")]
        [SwaggerResponse(200, "Accounts currently in a game, empty when nobody plays.", typeof(List<LoLLiveGameDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLiveGames(bool includeSmurfs = true, bool includeOutOfCrew = false)
        {
            return this.Ok(await this.mediator.Send(new GetLoLLiveGamesQuery
            {
                IncludeSmurfs = includeSmurfs,
                IncludeOutOfCrew = includeOutOfCrew,
            }));
        }
    }
}
