// <copyright file="HighlightController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using YuGames.Business.Contracts;
    using YuGames.Entities;

    /// <summary>
    /// Highlight Controller.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class HighlightController : ControllerBase
    {
        private IHighlightBusiness highlightBusi;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightController"/> class.
        /// </summary>
        /// <param name="highlightBusi">Highlight business interface (injected).</param>
        public HighlightController(IHighlightBusiness highlightBusi)
        {
            this.highlightBusi = highlightBusi;
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
    }
}