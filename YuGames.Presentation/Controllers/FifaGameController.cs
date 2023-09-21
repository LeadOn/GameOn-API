// <copyright file="FifaGameController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.FifaGamePlayed.Queries.GetFifaGamePlayedByTournamentId;
    using YuGames.Common.DTOs;

    /// <summary>
    /// FifaGame Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class FifaGameController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FifaGameController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public FifaGameController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets tournament games.
        /// </summary>
        /// <param name="tournamentId">Tournament ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("tournament/{tournamentId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get games played by tournament.")]
        [SwaggerResponse(200, "List of games played.", typeof(List<FifaGamePlayedDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetTournamentGames(int tournamentId)
        {
            return this.Ok(await this.mediator.Send(new GetFifaGamePlayedByTournamentIdQuery { TournamentId = tournamentId }));
        }
    }
}
