// <copyright file="GetAllQueuesQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Queues.Queries.GetAllQueues
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllQueuesQueryHandler class.
    /// </summary>
    public class GetAllQueuesQueryHandler : IRequestHandler<GetAllQueuesQuery, IEnumerable<LoLQueue>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllQueuesQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllQueuesQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LoLQueue>> Handle(GetAllQueuesQuery request, CancellationToken cancellationToken)
        {
            return await this.context.LeagueOfLegendsQueues.ToListAsync(cancellationToken);
        }
    }
}
