// <copyright file="GetLatestChangelogQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Queries.GetLatestChangelog
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLatestChangelogQueryHandler class.
    /// </summary>
    public class GetLatestChangelogQueryHandler : IRequestHandler<GetLatestChangelogQuery, Changelog>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestChangelogQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetLatestChangelogQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Changelog> Handle(GetLatestChangelogQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Changelogs.OrderByDescending(x => x.PublicationDate).FirstAsync(cancellationToken);
        }
    }
}
