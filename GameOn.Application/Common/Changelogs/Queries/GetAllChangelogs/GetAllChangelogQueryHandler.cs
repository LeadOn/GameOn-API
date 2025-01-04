// <copyright file="GetAllChangelogQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Queries.GetAllChangelogs
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllChangelogQueryHandler class.
    /// </summary>
    public class GetAllChangelogQueryHandler : IRequestHandler<GetAllChangelogQuery, IEnumerable<Changelog>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllChangelogQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllChangelogQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Changelog>> Handle(GetAllChangelogQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Changelogs.OrderByDescending(x => x.PublicationDate).ToListAsync(cancellationToken);
        }
    }
}
