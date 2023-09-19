// <copyright file="PlayerController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.Players.Queries.GetAllPlayers;
    using YuGames.Application.Players.Queries.GetConnectedPlayer;
    using YuGames.Application.Players.Queries.GetPlayerById;
    using YuGames.Domain;
    using YuGames.Presentation.Classes;

    /// <summary>
    /// Player Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public PlayerController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets connected user from Database.
        /// </summary>
        /// <returns>200 OK with user account, 401 if not authorized, 500 if something bad happens.</returns>
        [HttpGet]
        [Authorize]
        [Route("me")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Gets current user profile.", Description = "Gets current user profile as stored in database. If the user account doesn't exists, it creates it automatically.")]
        [SwaggerResponse(200, "Current user profile.", typeof(Player))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetConnected()
        {
            return this.Ok(await this.mediator.Send(new GetConnectedPlayerQuery { ConnectedPlayer = this.User.GetConnectedPlayer() }));
        }

        /// <summary>
        /// Get a player by its ID.
        /// </summary>
        /// <param name="id">Player ID.</param>
        /// <returns>200 OK with Player if found, 404 if not found.</returns>
        [HttpGet]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a player by its ID.", Description = "Get a player by its ID, and retrieve its information.")]
        [SwaggerResponse(200, "Player is found.", typeof(Player))]
        [SwaggerResponse(404, "Player not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int id)
        {
            var playerInDb = await this.mediator.Send(new GetPlayerByIdQuery { PlayerId = id });

            if (playerInDb is not null)
            {
                return this.Ok(playerInDb);
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
        [SwaggerOperation(Summary = "Get all players in database.")]
        [SwaggerResponse(200, "Players in database.", typeof(List<Player>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.mediator.Send(new GetAllPlayersQuery()));
        }
    }
}
