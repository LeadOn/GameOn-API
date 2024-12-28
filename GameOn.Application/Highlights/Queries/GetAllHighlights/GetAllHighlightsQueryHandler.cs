// <copyright file="GetAllHighlightsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Highlights.Queries.GetAllHighlights
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllHighlightsQueryHandler class.
    /// </summary>
    public class GetAllHighlightsQueryHandler : IRequestHandler<GetAllHighlightsQuery, IEnumerable<Highlight>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllHighlightsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllHighlightsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Highlight>> Handle(GetAllHighlightsQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Highlights.ToListAsync();
        }
    }
}
