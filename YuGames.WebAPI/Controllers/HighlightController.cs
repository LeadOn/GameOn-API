// <copyright file="HighlightController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Business.Contracts;
    using YuGames.DTOs;
    using YuGames.Entities;
    using YuGames.WebAPI.Classes;

    /// <summary>
    /// Highlight Controller.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class HighlightController : ControllerBase
    {
        private IHighlightBusiness highlightBusi;
        private IPlayerBusiness playerBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightController"/> class.
        /// </summary>
        /// <param name="highlightBusi">Highlight business interface (injected).</param>
        /// <param name="playerBusi">Player business interface (injected).</param>
        public HighlightController(IHighlightBusiness highlightBusi, IPlayerBusiness playerBusi)
        {
            this.highlightBusi = highlightBusi;
            this.playerBusi = playerBusi;
        }

        /// <summary>
        /// Get all highlights in database.
        /// </summary>
        /// <returns>200 OK with Highlight list.</returns>
        [HttpGet]
        [Route("all")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all Highlights in database.")]
        [SwaggerResponse(200, "Highlights in database.", typeof(List<Highlight>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.highlightBusi.GetAll());
        }

        /// <summary>
        /// Creates highlight in database.
        /// </summary>
        /// <param name="highlight"><see cref="CreateHighlightDto"/>.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Creates Highlight in database.", Description = "Creates a highlight in database that is linked to a FIFA Game Played.")]
        [SwaggerResponse(201, "Highlight created.", typeof(Highlight))]
        [SwaggerResponse(401, "Unauthorized.")]
        [SwaggerResponse(500, "Something wrong happened.")]
        public async Task<IActionResult> Create([FromBody] CreateHighlightDto highlight)
        {
            // Getting connected user
            var connectedPlayer = new ConnectedPlayerDto
            {
                Email = this.User.GetUserEmail(),
                KeycloakId = this.User.GetUserId(),
                FirstName = this.User.GetUserFirstName(),
                LastName = this.User.GetUserLastName(),
                PreferredUsername = this.User.GetUserPreferredName(),
            };

            if (highlight.Description == string.Empty)
            {
                highlight.Description = null;
            }

            if (highlight.ExternalUrl == string.Empty)
            {
                highlight.ExternalUrl = null;
            }

            var playerInDb = await this.playerBusi.GetConnectedUser(connectedPlayer);

            return this.Ok(await this.highlightBusi.Create(highlight.Name, highlight.Description, playerInDb.Id, highlight.FifaGameId, highlight.ExternalUrl));
        }
    }
}