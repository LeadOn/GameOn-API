// <copyright file="PlayerController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuFoot.Business.Contracts;
    using YuFoot.Entities;

    /// <summary>
    /// Player Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> logger;
        private IPlayerBusiness playerBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="logger">Logger interface (injected).</param>
        /// <param name="player">Player business interface (injected).</param>
        public PlayerController(ILogger<PlayerController> logger, IPlayerBusiness player)
        {
            this.logger = logger;
            this.playerBusi = player;
        }

        /// <summary>
        /// Get a player by its ID.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <returns>200 OK with Player if found, 404 if not found.</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a player by its ID.", Description = "Get a player by its ID, and retrieve its information.")]
        [SwaggerResponse(200, "Player is found.", typeof(Player))]
        [SwaggerResponse(404, "Player not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int id)
        {
            var player = await this.playerBusi.GetPlayerById(id);

            if (player is not null)
            {
                return this.Ok(player);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get all players in database.
        /// </summary>
        /// <returns>200 OK with Player list.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all player in database.")]
        [SwaggerResponse(200, "Players in database.", typeof(List<Player>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.playerBusi.GetAll());
        }
    }
}