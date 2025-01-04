// <copyright file="GetAllSeasonsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Seasons.Queries.GetAllSeasons
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllSeasonsQueryHandler class.
    /// </summary>
    public class GetAllSeasonsQueryHandler : IRequestHandler<GetAllSeasonsQuery, IEnumerable<Season>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSeasonsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllSeasonsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Season>> Handle(GetAllSeasonsQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Seasons.ToListAsync();
        }
    }
}
