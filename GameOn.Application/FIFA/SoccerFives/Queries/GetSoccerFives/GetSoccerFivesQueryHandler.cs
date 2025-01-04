// <copyright file="GetSoccerFivesQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.SoccerFives.Queries.GetSoccerFives
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetSoccerFivesQueryHandler class.
    /// </summary>
    public class GetSoccerFivesQueryHandler : IRequestHandler<GetSoccerFivesQuery, List<SoccerFive>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSoccerFivesQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetSoccerFivesQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<List<SoccerFive>> Handle(GetSoccerFivesQuery request, CancellationToken cancellationToken)
        {
            return await context.SoccerFives.Include(x => x.CreatedBy).ToListAsync(cancellationToken);
        }
    }
}
