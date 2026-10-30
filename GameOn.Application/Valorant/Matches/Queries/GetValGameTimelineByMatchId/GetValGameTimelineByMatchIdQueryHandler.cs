// <copyright file="GetValGameTimelineByMatchIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Queries.GetValGameTimelineByMatchId
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="GetValGameTimelineByMatchIdQuery"/> handler.
    /// </summary>
    public class GetValGameTimelineByMatchIdQueryHandler : IRequestHandler<GetValGameTimelineByMatchIdQuery, ValGameTimelineDto?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValGameTimelineByMatchIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValGameTimelineByMatchIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<ValGameTimelineDto?> Handle(GetValGameTimelineByMatchIdQuery request, CancellationToken cancellationToken)
        {
            var gameExists = await this.context.ValorantGames
                .AnyAsync(x => x.MatchId == request.MatchId, cancellationToken);

            if (!gameExists)
            {
                return null;
            }

            var rounds = await this.context.ValorantGameRounds
                .Where(x => x.MatchId == request.MatchId)
                .OrderBy(x => x.RoundNumber)
                .ToListAsync(cancellationToken);

            var kills = await this.context.ValorantGameKills
                .Where(x => x.MatchId == request.MatchId)
                .OrderBy(x => x.RoundNumber)
                .ThenBy(x => x.TimeInRoundMs)
                .ThenBy(x => x.TimeInMatchMs)
                .ThenBy(x => x.Id)
                .ToListAsync(cancellationToken);

            var roundPlayerStats = await this.context.ValorantGameRoundPlayerStats
                .Where(x => x.MatchId == request.MatchId)
                .OrderBy(x => x.RoundNumber)
                .ThenBy(x => x.Team)
                .ThenBy(x => x.Puuid)
                .ToListAsync(cancellationToken);

            return new ValGameTimelineDto
            {
                MatchId = request.MatchId,
                Rounds = rounds,
                Kills = kills,
                RoundPlayerStats = roundPlayerStats,
            };
        }
    }
}
