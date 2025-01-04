// <copyright file="SearchTeamPlayerQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.TeamPlayers.Queries.SearchTeamPlayer
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// SearchTeamPlayerQueryHandler class.
    /// </summary>
    public class SearchTeamPlayerQueryHandler : IRequestHandler<SearchTeamPlayerQuery, IEnumerable<FifaTeamPlayer>>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchTeamPlayerQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public SearchTeamPlayerQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaTeamPlayer>> Handle(SearchTeamPlayerQuery request, CancellationToken cancellationToken)
        {
            return await this.context.FifaTeamPlayers.Where(request.Query).Take(request.Limit).ToListAsync(cancellationToken);
        }
    }
}
