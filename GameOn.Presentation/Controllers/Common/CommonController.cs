// <copyright file="CommonController.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Presentation.Controllers.Common
{
    using GameOn.Application.Common.Home.Queries.GetHomeData;
    using GameOn.Common.DTOs.Common;
    using GameOn.Presentation.Classes;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Common Controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR interface, injected.</param>
        public CommonController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all tournaments in database.
        /// </summary>
        /// <returns>IActionResult object.</returns>
        [HttpGet]
        [Route("home")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get data used in home page of GameOn! front-end.")]
        [SwaggerResponse(200, "Home data, dynamicaly calculated depending on connected user.", typeof(HomeDataDto))]
        [SwaggerResponse(500, "Unknown error happened.")]
        public async Task<IActionResult> GetHomeData()
        {
            return this.Ok(await this.mediator.Send(new GetHomeDataQuery()));
        }
    }
}
