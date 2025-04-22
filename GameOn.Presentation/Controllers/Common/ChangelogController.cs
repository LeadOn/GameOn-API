// <copyright file="ChangelogController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.Common
{
    using GameOn.Application.Common.Changelogs.Commands.CreateChangelog;
    using GameOn.Application.Common.Changelogs.Commands.DeleteChangelog;
    using GameOn.Application.Common.Changelogs.Commands.UpdateChangelog;
    using GameOn.Application.Common.Changelogs.Queries.GetAllChangelogs;
    using GameOn.Application.Common.Changelogs.Queries.GetChangelogById;
    using GameOn.Application.Common.Changelogs.Queries.GetLatestChangelog;
    using GameOn.Common.DTOs;
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
        /// Get latest changelog.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("latest")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get latest changelog from database.")]
        [SwaggerResponse(200, "Latest changelog.", typeof(Changelog))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetLatest()
        {
            return this.Ok(await this.mediator.Send(new GetLatestChangelogQuery()));
        }

        /// <summary>
        /// Get changelog by ID.
        /// </summary>
        /// <param name="id">Changelog ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("{id:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get changelog from database.")]
        [SwaggerResponse(200, "Changelog.", typeof(Changelog))]
        [SwaggerResponse(404, "Changelog not found in database.", typeof(Changelog))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int id)
        {
            var changelog = await this.mediator.Send(new GetChangelogByIdQuery { Id = id });

            if (changelog == null)
            {
                return this.NotFound();
            }
            else
            {
                return this.Ok(changelog);
            }
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
        public async Task<IActionResult> CreateChangelog(CreateChangelogDto changelog)
        {
            return this.Ok(await this.mediator.Send(new CreateChangelogCommand { Changelog = changelog }));
        }

        /// <summary>
        /// Update changelog.
        /// </summary>
        /// <param name="changelog">Changelog to update.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Route("")]
        [Authorize(Roles = "gameon_admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update changelog in database.")]
        [SwaggerResponse(200, "Updated changelog.", typeof(Changelog))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> UpdateChangelog(Changelog changelog)
        {
            return this.Ok(await this.mediator.Send(new UpdateChangelogCommand { Changelog = changelog }));
        }

        /// <summary>
        /// Delete changelog.
        /// </summary>
        /// <param name="id">Changelog to update.</param>
        /// <returns>IActionResult object.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "gameon_admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Delete changelog in database.")]
        [SwaggerResponse(204, "Deleted changelog.", typeof(Changelog))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> DeleteChangelog(int id)
        {
            var result = await this.mediator.Send(new DeleteChangelogCommand { Id = id });

            if (result == false)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
