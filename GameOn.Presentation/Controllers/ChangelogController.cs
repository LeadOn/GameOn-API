// <copyright file="ChangelogController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers
{
    using GameOn.Application.Changelogs.Commands.CreateChangelog;
    using GameOn.Application.Changelogs.Queries.GetAllChangelogs;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Changelog Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ChangelogController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangelogController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public ChangelogController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all changelogs.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all changelogs in database.")]
        [SwaggerResponse(200, "List of changelogs.", typeof(List<Changelog>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.mediator.Send(new GetAllChangelogQuery()));
        }

        /// <summary>
        /// Create changelog.
        /// </summary>
        /// <param name="changelog">Changelog to create.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "gameon_admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create changelog in database.")]
        [SwaggerResponse(200, "Created changelog.", typeof(Changelog))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll(Changelog changelog)
        {
            return this.Ok(await this.mediator.Send(new CreateChangelogCommand { Changelog = changelog }));
        }
    }
}
