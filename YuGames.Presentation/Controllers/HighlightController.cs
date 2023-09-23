// <copyright file="HighlightController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.Highlights.Commands.CreateHighlight;
    using YuGames.Application.Highlights.Queries.GetAllHighlights;
    using YuGames.Application.Players.Queries.GetConnectedPlayer;
    using YuGames.Common.DTOs;
    using YuGames.Domain;
    using YuGames.Presentation.Classes;

    /// <summary>
    /// Highlight Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HighlightController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public HighlightController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all highlights in database.
        /// </summary>
        /// <returns>200 OK with Highlight list.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all Highlights in database.")]
        [SwaggerResponse(200, "Highlights in database.", typeof(List<Highlight>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.mediator.Send(new GetAllHighlightsQuery()));
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
            if (highlight.Description == string.Empty)
            {
                highlight.Description = null;
            }

            if (highlight.ExternalUrl == string.Empty)
            {
                highlight.ExternalUrl = null;
            }

            var playerInDb = await this.mediator.Send(new GetConnectedPlayerQuery
                { ConnectedPlayer = this.User.GetConnectedPlayer() });

            if (playerInDb is null)
            {
                return this.NotFound();
            }

            return this.Ok(await this.mediator.Send(new CreateHighlightCommand
            {
                Highlight = new Highlight
                {
                    Name = highlight.Name,
                    Description = highlight.Description,
                    ExternalUrl = highlight.ExternalUrl,
                    FifaGameId = highlight.FifaGameId,
                    CreatedById = playerInDb.Id,
                },
            }));
        }
    }
}
