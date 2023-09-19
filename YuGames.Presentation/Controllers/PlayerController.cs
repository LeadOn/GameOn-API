// <copyright file="PlayerController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.Player.Queries.GetConnectedPlayer;
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
    }
}
