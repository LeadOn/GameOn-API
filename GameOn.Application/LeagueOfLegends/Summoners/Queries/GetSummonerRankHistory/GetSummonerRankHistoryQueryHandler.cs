// <copyright file="GetSummonerRankHistoryQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetSummonerRankHistory
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetSummonerRankHistoryQueryHandler class.
    /// </summary>
    public class GetSummonerRankHistoryQueryHandler : IRequestHandler<GetSummonerRankHistoryQuery, IEnumerable<LeagueOfLegendsRankHistory>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSummonerRankHistoryQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetSummonerRankHistoryQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LeagueOfLegendsRankHistory>> Handle(GetSummonerRankHistoryQuery request, CancellationToken cancellationToken)
        {
            return await this.context.LeagueOfLegendsRankHistory
                .Where(x => x.PlayerId == request.PlayerId)
                .OrderBy(x => x.CreatedOn)
                .Take(request.Limit is not null ? (int)request.Limit : 50)
                .ToListAsync(cancellationToken);
        }
    }
}
