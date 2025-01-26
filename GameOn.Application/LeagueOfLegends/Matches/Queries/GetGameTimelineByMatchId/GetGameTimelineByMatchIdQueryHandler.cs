// <copyright file="GetGameTimelineByMatchIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetGameTimelineByMatchId
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetGameTimelineByMatchIdQueryHandler class.
    /// </summary>
    public class GetGameTimelineByMatchIdQueryHandler : IRequestHandler<GetGameTimelineByMatchIdQuery, List<LoLGameTimelineFrame>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGameTimelineByMatchIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetGameTimelineByMatchIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<List<LoLGameTimelineFrame>> Handle(GetGameTimelineByMatchIdQuery request, CancellationToken cancellationToken)
        {
            return await this.context.LeagueOfLegendsGameTimelineFrames
                .Include(x => x.LoLGameTimelineFrameParticipants)
                .OrderBy(x => x.Timestamp)
                .Where(x => x.MatchId == request.MatchId)
                .ToListAsync(cancellationToken);
        }
    }
}
