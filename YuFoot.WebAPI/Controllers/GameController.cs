// <copyright file="GameController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuFoot.Business.Contracts;
    using YuFoot.DTOs;

    /// <summary>
    /// Game Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> logger;
        private IGamePlayedBusiness gamePlayedBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController"/> class.
        /// </summary>
        /// <param name="logger">Logger interface (injected).</param>
        /// <param name="gamePlayed">GamePlayed business interface (injected).</param>
        public GameController(ILogger<GameController> logger, IGamePlayedBusiness gamePlayed)
        {
            this.logger = logger;
            this.gamePlayedBusi = gamePlayed;
        }

        /// <summary>
        /// Get last 5 games played.
        /// </summary>
        /// <returns>200 OK with Game list.</returns>
        [HttpGet]
        [Route("last")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get last 5 games played.", Description = "Get last 5 games played with their players.")]
        [SwaggerResponse(200, "List of games played.", typeof(List<GamePlayedDto>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLastFiveGames()
        {
            return this.Ok(await this.gamePlayedBusi.GetLastFiveGames());
        }
    }
}