// <copyright file="PlatformController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuFoot.Business.Contracts;
    using YuFoot.Entities;

    /// <summary>
    /// Platform Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
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
        [Route("")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all platforms in database.")]
        [SwaggerResponse(200, "Platform in database.", typeof(List<Player>))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetAll()
        {
            return this.Ok(await this.platformBusi.GetAll());
        }
    }
}