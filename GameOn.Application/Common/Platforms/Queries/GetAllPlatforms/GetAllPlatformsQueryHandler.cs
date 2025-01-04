// <copyright file="GetAllPlatformsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Platforms.Queries.GetAllPlatforms
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllPlatformsQueryHandler class.
    /// </summary>
    public class GetAllPlatformsQueryHandler : IRequestHandler<GetAllPlatformsQuery, IEnumerable<Platform>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllPlatformsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllPlatformsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Platform>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Platforms.ToListAsync();
        }
    }
}
