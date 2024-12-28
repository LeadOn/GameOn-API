// <copyright file="GetPlatformByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Platforms.Queries.GetPlatformById
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetPlatformByIdQueryHandler class.
    /// </summary>
    public class GetPlatformByIdQueryHandler : IRequestHandler<GetPlatformByIdQuery, Platform?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPlatformByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetPlatformByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Platform?> Handle(GetPlatformByIdQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Platforms.FirstOrDefaultAsync(x => x.Id == request.PlatformId);
        }
    }
}
