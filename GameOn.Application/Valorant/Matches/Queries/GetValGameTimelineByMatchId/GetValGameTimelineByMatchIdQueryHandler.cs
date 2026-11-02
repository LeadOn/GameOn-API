// <copyright file="GetValGameTimelineByMatchIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Queries.GetValGameTimelineByMatchId
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;
    using GameOn.External.ValorantApi.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="GetValGameTimelineByMatchIdQuery"/> handler.
    /// </summary>
    public class GetValGameTimelineByMatchIdQueryHandler : IRequestHandler<GetValGameTimelineByMatchIdQuery, ValGameTimelineDto?>
    {
        private readonly IApplicationDbContext context;
        private readonly IValorantMapService mapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValGameTimelineByMatchIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mapService">Valorant map reference data, injected.</param>
        public GetValGameTimelineByMatchIdQueryHandler(IApplicationDbContext context, IValorantMapService mapService)
        {
            this.context = context;
            this.mapService = mapService;
        }

        /// <inheritdoc />
        public async Task<ValGameTimelineDto?> Handle(GetValGameTimelineByMatchIdQuery request, CancellationToken cancellationToken)
        {
            var mapId = await this.context.ValorantGames
                .Where(x => x.MatchId == request.MatchId)
                .Select(x => new { x.MapId })
                .FirstOrDefaultAsync(cancellationToken);

            if (mapId is null)
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

            var playerLocations = await this.context.ValorantGameEventPlayerLocations
                .Where(x => x.MatchId == request.MatchId)
                .OrderBy(x => x.RoundNumber)
                .ThenBy(x => x.ValGameKillId)
                .ThenBy(x => x.Id)
                .ToListAsync(cancellationToken);

            var damages = await this.context.ValorantGameRoundDamages
                .Where(x => x.MatchId == request.MatchId)
                .OrderBy(x => x.RoundNumber)
                .ThenBy(x => x.AttackerPuuid)
                .ThenBy(x => x.ReceiverPuuid)
                .ToListAsync(cancellationToken);

            var maps = await this.mapService.GetMapsById(cancellationToken);
            var map = mapId.MapId is not null ? maps.GetValueOrDefault(mapId.MapId) : null;

            return new ValGameTimelineDto
            {
                MatchId = request.MatchId,
                Rounds = rounds,
                Kills = kills,
                RoundPlayerStats = roundPlayerStats,
                PlayerLocations = playerLocations,
                Damages = damages,
                MapTransform = map is null || (map.XMultiplier == 0 && map.YMultiplier == 0)
                    ? null
                    : new ValMapTransformDto
                    {
                        MinimapUrl = map.DisplayIcon,
                        XMultiplier = map.XMultiplier,
                        YMultiplier = map.YMultiplier,
                        XScalarToAdd = map.XScalarToAdd,
                        YScalarToAdd = map.YScalarToAdd,
                    },
            };
        }
    }
}
