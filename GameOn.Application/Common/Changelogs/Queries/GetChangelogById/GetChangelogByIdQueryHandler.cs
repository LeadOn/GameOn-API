// <copyright file="GetChangelogByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Queries.GetChangelogById
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetChangelogByIdQueryHandler class.
    /// </summary>
    public class GetChangelogByIdQueryHandler : IRequestHandler<GetChangelogByIdQuery, Changelog?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChangelogByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetChangelogByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Changelog?> Handle(GetChangelogByIdQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Changelogs.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}
