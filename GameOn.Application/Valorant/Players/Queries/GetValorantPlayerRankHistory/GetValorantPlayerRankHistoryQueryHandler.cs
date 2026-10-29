// <copyright file="GetValorantPlayerRankHistoryQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerRankHistory
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetValorantPlayerRankHistoryQueryHandler class.
    /// </summary>
    public class GetValorantPlayerRankHistoryQueryHandler : IRequestHandler<GetValorantPlayerRankHistoryQuery, IEnumerable<ValorantRankHistory>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValorantPlayerRankHistoryQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValorantPlayerRankHistoryQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ValorantRankHistory>> Handle(GetValorantPlayerRankHistoryQuery request, CancellationToken cancellationToken)
        {
            if (request.Granularity is not null)
            {
                return await this.GetBucketedHistory(request, cancellationToken);
            }

            var limit = request.Limit ?? 50;

            var recentEntries = await this.context.ValorantRankHistory
                .Where(x => x.PlayerId == request.PlayerId)
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return recentEntries
                .OrderBy(x => x.CreatedOn)
                .ThenBy(x => x.Id)
                .ToList();
        }

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

        private static List<ValorantRankHistory> FillGaps(List<ValorantRankHistory> history, DateTime since, DateTime now, LoLRankHistoryGranularity granularity)
        {
            var buckets = new List<(long Key, DateTime RepresentativeDate)>();

            for (var day = since.Date; day <= now.Date; day = day.AddDays(1))
            {
                var key = GetBucketKey(day, granularity);

                if (buckets.Count > 0 && buckets[^1].Key == key)
                {
                    buckets[^1] = (key, day);
                }
                else
                {
                    buckets.Add((key, day));
                }
            }

            var results = new List<ValorantRankHistory>();
            ValorantRankHistory? lastKnown = null;
            var historyIndex = 0;

            foreach (var bucket in buckets)
            {
                while (historyIndex < history.Count && GetBucketKey(history[historyIndex].CreatedOn, granularity) < bucket.Key)
                {
                    lastKnown = history[historyIndex];
                    historyIndex++;
                }

                ValorantRankHistory? localLatest = null;

                while (historyIndex < history.Count && GetBucketKey(history[historyIndex].CreatedOn, granularity) == bucket.Key)
                {
                    localLatest = history[historyIndex];
                    historyIndex++;
                }

                if (localLatest is not null)
                {
                    lastKnown = localLatest;
                    results.Add(localLatest);
                }
                else if (lastKnown is not null)
                {
                    results.Add(new ValorantRankHistory
                    {
                        PlayerId = lastKnown.PlayerId,
                        MatchId = lastKnown.MatchId,
                        SeasonId = lastKnown.SeasonId,
                        SeasonShort = lastKnown.SeasonShort,
                        TierId = lastKnown.TierId,
                        TierName = lastKnown.TierName,
                        RankedRating = lastKnown.RankedRating,
                        Elo = lastKnown.Elo,
                        LastChange = 0,
                        RefundedRr = 0,
                        WasDerankProtected = false,
                        MapId = lastKnown.MapId,
                        MapName = lastKnown.MapName,
                        CreatedOn = bucket.RepresentativeDate,
                    });
                }
            }

            return results;
        }

        private async Task<IEnumerable<ValorantRankHistory>> GetBucketedHistory(GetValorantPlayerRankHistoryQuery request, CancellationToken cancellationToken)
        {
            var granularity = request.Granularity!.Value;

            var days = request.Days ?? granularity switch
            {
                LoLRankHistoryGranularity.Day => 21,
                LoLRankHistoryGranularity.Week => 90,
                LoLRankHistoryGranularity.Month => 365,
                _ => 30,
            };

            var now = DateTime.UtcNow;
            var since = now.AddDays(-days);

            var history = await this.context.ValorantRankHistory
                .Where(x => x.PlayerId == request.PlayerId)
                .OrderBy(x => x.CreatedOn)
                .ThenBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return FillGaps(history, since, now, granularity);
        }
    }
}