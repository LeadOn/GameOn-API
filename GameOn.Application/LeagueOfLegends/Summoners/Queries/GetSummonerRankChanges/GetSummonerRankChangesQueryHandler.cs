// <copyright file="GetSummonerRankChangesQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetSummonerRankChanges
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetSummonerRankChangesQueryHandler class.
    /// </summary>
    public class GetSummonerRankChangesQueryHandler : IRequestHandler<GetSummonerRankChangesQuery, List<LoLGameRankChangeDto>>
    {
        // Match-v5 queue IDs for the two ranked queues (see GetLeaguePlayerByIdQueryHandler, which uses the
        // same constants).
        private const int SoloQueueId = 420;
        private const int FlexQueueId = 440;

        private const int DefaultLimit = 50;

        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSummonerRankChangesQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetSummonerRankChangesQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<List<LoLGameRankChangeDto>> Handle(GetSummonerRankChangesQuery request, CancellationToken cancellationToken)
        {
            var queueIds = request.Queue switch
            {
                LoLQueueFilter.Solo => new List<int> { SoloQueueId },
                LoLQueueFilter.Flex => new List<int> { FlexQueueId },
                _ => new List<int> { SoloQueueId, FlexQueueId },
            };

            // Every ranked game is listed, including the ones whose LP couldn't be pinned (RankChange null):
            // dropping them would put two games side by side on a chart when others were played in between.
            // Remakes are the exception, they move no LP for the players who stayed and would only be noise.
            // An empty champion name is a placeholder left by an import that never completed.
            var query = this.context.LeagueOfLegendsGameParticipants
                .AsNoTracking()
                .Where(x => x.PlayerId == request.PlayerId
                    && x.Game.QueueId != null
                    && queueIds.Contains(x.Game.QueueId.Value)
                    && !x.Game.IsRemake
                    && x.ChampionName != string.Empty);

            if (request.Days is not null)
            {
                var since = DateTime.UtcNow.AddDays(-request.Days.Value);
                query = query.Where(x => x.Game.GameStart >= since);
            }

            var games = await query
                .OrderByDescending(x => x.Game.GameStart)
                .Take(request.Limit ?? DefaultLimit)
                .Select(x => new LoLGameRankChangeDto
                {
                    MatchId = x.MatchId,
                    QueueId = x.Game.QueueId!.Value,
                    GameStart = x.Game.GameStart,
                    Win = x.Win,
                    ChampionName = x.ChampionName,
                    Kills = x.Kills,
                    Deaths = x.Deaths,
                    Assists = x.Assists,
                    RankChange = x.RankChange,
                })
                .ToListAsync(cancellationToken);

            // Oldest first, like the rank history: this is meant to be read left to right on a chart.
            return games.OrderBy(x => x.GameStart).ToList();
        }
    }
}
