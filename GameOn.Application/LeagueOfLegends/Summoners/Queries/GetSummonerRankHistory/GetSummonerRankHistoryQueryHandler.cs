// <copyright file="GetSummonerRankHistoryQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetSummonerRankHistory
{
    using System.Globalization;
    using GameOn.Common.DTOs.LeagueOfLegends;
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
            if (request.Granularity is not null)
            {
                return await this.GetBucketedHistory(request, cancellationToken);
            }

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

            var limit = request.Limit ?? 50;

            return soloHistoryToRetrieve.Take(limit).ToList().Concat(flexHistoryToRetrieve.Take(limit).ToList()).OrderBy(x => x.CreatedOn).ToList();
        }

        /// <summary>
        /// Builds a comparable key that groups a date down to the given granularity's period.
        /// </summary>
        private static long GetBucketKey(DateTime date, LoLRankHistoryGranularity granularity)
        {
            return granularity switch
            {
                LoLRankHistoryGranularity.Day => (date.Year * 10000L) + (date.Month * 100) + date.Day,
                LoLRankHistoryGranularity.Week => (ISOWeek.GetYear(date) * 100L) + ISOWeek.GetWeekOfYear(date),
                LoLRankHistoryGranularity.Month => (date.Year * 100L) + date.Month,
                _ => throw new ArgumentOutOfRangeException(nameof(granularity), granularity, null),
            };
        }

        /// <summary>
        /// Keeps only the last snapshot of each period (day/week/month) per queue, over a rolling window.
        /// </summary>
        private async Task<IEnumerable<LeagueOfLegendsRankHistory>> GetBucketedHistory(GetSummonerRankHistoryQuery request, CancellationToken cancellationToken)
        {
            var granularity = request.Granularity!.Value;

            var days = request.Days ?? granularity switch
            {
                LoLRankHistoryGranularity.Day => 21,
                LoLRankHistoryGranularity.Week => 90,
                LoLRankHistoryGranularity.Month => 365,
                _ => 30,
            };

            var since = DateTime.Now.AddDays(-days);

            var history = await this.context.LeagueOfLegendsRankHistory
                .Where(x => x.PlayerId == request.PlayerId
                    && x.CreatedOn >= since
                    && (x.QueueType == "RANKED_SOLO_5x5" || x.QueueType == "RANKED_FLEX_SR"))
                .OrderBy(x => x.CreatedOn)
                .ToListAsync(cancellationToken);

            return history
                .GroupBy(x => (x.QueueType, Bucket: GetBucketKey(x.CreatedOn, granularity)))
                .Select(g => g.OrderByDescending(x => x.CreatedOn).First())
                .OrderBy(x => x.CreatedOn)
                .ToList();
        }
    }
}
