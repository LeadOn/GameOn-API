// <copyright file="GetSummonerRankHistoryQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetSummonerRankHistory
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Options;

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
            var historySoloQueue = await this.context.LeagueOfLegendsRankHistory
                .Where(x => x.PlayerId == request.PlayerId && x.QueueType == "RANKED_SOLO_5x5")
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync(cancellationToken);

            var historyFlexQueue = await this.context.LeagueOfLegendsRankHistory
                .Where(x => x.PlayerId == request.PlayerId && x.QueueType == "RANKED_FLEX_SR")
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync(cancellationToken);

            var soloHistoryToRetrieve = new List<LeagueOfLegendsRankHistory>();
            var flexHistoryToRetrieve = new List<LeagueOfLegendsRankHistory>();

            foreach (var item in historySoloQueue)
            {
                if (soloHistoryToRetrieve.Count == 0)
                {
                    soloHistoryToRetrieve.Add(item);
                }
                else
                {
                    var lastItem = soloHistoryToRetrieve.LastOrDefault();

                    if (lastItem is null)
                    {
                        soloHistoryToRetrieve.Add(item);
                    }
                    else if (lastItem.Wins != item.Wins || lastItem.Losses != item.Losses)
                    {
                        soloHistoryToRetrieve.Add(item);
                    }
                }
            }

            foreach (var item in historyFlexQueue)
            {
                if (flexHistoryToRetrieve.Count == 0)
                {
                    flexHistoryToRetrieve.Add(item);
                }
                else
                {
                    var lastItem = flexHistoryToRetrieve.LastOrDefault();

                    if (lastItem is null)
                    {
                        flexHistoryToRetrieve.Add(item);
                    }
                    else if (lastItem.Wins != item.Wins && lastItem.Losses != item.Losses)
                    {
                        flexHistoryToRetrieve.Add(item);
                    }
                }
            }

            return soloHistoryToRetrieve.Concat(flexHistoryToRetrieve).Take(request.Limit is not null ? (int)request.Limit : 50).OrderBy(x => x.CreatedOn);
        }
    }
}
