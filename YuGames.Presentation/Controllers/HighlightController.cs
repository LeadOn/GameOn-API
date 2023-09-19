// <copyright file="HighlightController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Presentation.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Application.Highlights.Queries.GetAllHighlights;
    using YuGames.Domain;

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
    }
}
