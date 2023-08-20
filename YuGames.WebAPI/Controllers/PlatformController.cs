// <copyright file="PlatformController.cs" company="LeadOn's Corp'">
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

    /// <summary>
    /// Platform Controller.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PlatformController : ControllerBase
    {
        private IPlatformBusiness platformBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformController"/> class.
        /// </summary>
        /// <param name="platform">Platform business interface (injected).</param>
        public PlatformController(IPlatformBusiness platform)
        {
            this.platformBusi = platform;
        }

        /// <summary>
        /// Get all platforms in database.
        /// </summary>
        /// <returns>200 OK with Platform list.</returns>
        [HttpGet]
        [Route("all")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all gaming platforms in database.")]
        [SwaggerResponse(200, "Platform in database.", typeof(List<Player>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.platformBusi.GetAll());
        }

        /// <summary>
        /// Creates platform in database.
        /// </summary>
        /// <param name="platform"><see cref="Platform" />.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPost]
        [Authorize(Roles = "yugames_admin")]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Creates a platform in database.")]
        [SwaggerResponse(200, "Created platform.", typeof(Platform))]
        [SwaggerResponse(401, "User is not logged in.")]
        [SwaggerResponse(403, "User isn't admin.")]
        [SwaggerResponse(500, "Unknown error.")]
        public async Task<IActionResult> Create([FromBody] Platform platform)
        {
            var platformCreated = await this.platformBusi.Create(platform);

            if (platformCreated is null)
            {
                return this.Problem();
            }
            else
            {
                return this.Ok(platformCreated);
            }
        }

        /// <summary>
        /// Updates platform in database.
        /// </summary>
        /// <param name="platform"><see cref="Platform" />.</param>
        /// <returns>IActionResult object.</returns>
        [HttpPatch]
        [Authorize(Roles = "yugames_admin")]
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Updates a platform in database.")]
        [SwaggerResponse(200, "Updated platform.", typeof(Platform))]
        [SwaggerResponse(401, "User is not logged in.")]
        [SwaggerResponse(403, "User isn't admin.")]
        [SwaggerResponse(500, "Unknown error.")]
        public async Task<IActionResult> Update([FromBody] Platform platform)
        {
            var updatedPlatform = await this.platformBusi.Update(platform);

            if (updatedPlatform is null)
            {
                return this.Problem();
            }
            else
            {
                return this.Ok(updatedPlatform);
            }
        }

        /// <summary>
        /// Get platform by ID.
        /// </summary>
        /// <param name="platformId">Platform ID.</param>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("{platformId:int}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get platform by ID.")]
        [SwaggerResponse(200, "Platform.", typeof(Platform))]
        [SwaggerResponse(404, "Platform not found.")]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetById(int platformId)
        {
            var platformInDb = await this.platformBusi.GetById(platformId);

            if (platformInDb is null)
            {
                return this.NotFound();
            }

            return this.Ok(platformInDb);
        }
    }
}