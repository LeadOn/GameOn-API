// <copyright file="GetFeaturedTournamentsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Tournaments.Queries.GetFeaturedTournaments
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetFeaturedTournamentsQueryHandler class.
    /// </summary>
    public class GetFeaturedTournamentsQueryHandler : IRequestHandler<GetFeaturedTournamentsQuery, IEnumerable<TournamentDto>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFeaturedTournamentsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetFeaturedTournamentsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TournamentDto>> Handle(GetFeaturedTournamentsQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Tournaments
                .Where(x => x.Featured == true && (x.State == TournamentStates.Phase1 || x.State == TournamentStates.Phase2))
                .OrderBy(x => x.PlannedFrom)
                .Select(x => new TournamentDto(x))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
