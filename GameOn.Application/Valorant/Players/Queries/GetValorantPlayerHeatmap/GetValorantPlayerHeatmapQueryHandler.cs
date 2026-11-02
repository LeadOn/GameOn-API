// <copyright file="GetValorantPlayerHeatmapQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerHeatmap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Services;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;
    using GameOn.External.ValorantApi.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="GetValorantPlayerHeatmapQuery"/> handler. A kill is placed where the player stood when they
    /// shot, a death where they fell. Matches imported before positions were stored have no point until they
    /// are re-imported.
    /// </summary>
    public class GetValorantPlayerHeatmapQueryHandler : IRequestHandler<GetValorantPlayerHeatmapQuery, ValHeatmapDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IValorantMapService mapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValorantPlayerHeatmapQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mapService">Valorant map reference data, injected.</param>
        public GetValorantPlayerHeatmapQueryHandler(IApplicationDbContext context, IValorantMapService mapService)
        {
            this.context = context;
            this.mapService = mapService;
        }

        /// <inheritdoc />
        public async Task<ValHeatmapDto> Handle(GetValorantPlayerHeatmapQuery request, CancellationToken cancellationToken)
        {
            DateTime? since = request.Period switch
            {
                LoLStatsPeriod.Week => DateTime.UtcNow.AddDays(-7),
                LoLStatsPeriod.Month => DateTime.UtcNow.AddMonths(-1),
                LoLStatsPeriod.ThreeMonths => DateTime.UtcNow.AddMonths(-3),
                LoLStatsPeriod.SixMonths => DateTime.UtcNow.AddMonths(-6),
                _ => null,
            };

            var mapName = request.MapName.Trim();
            var participations = this.context.ValorantGameParticipants
                .Where(x => x.PlayerId == request.PlayerId && x.Game.MapName == mapName)
                .CountedGamesOnly();

            if (since is not null)
            {
                participations = participations.Where(x => x.Game.GameStart >= since);
            }

            var queues = request.Queues?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToLowerInvariant())
                .Distinct()
                .ToList();

            if (queues is { Count: > 0 })
            {
                participations = participations.Where(x => x.Game.QueueId != null && queues.Contains(x.Game.QueueId));
            }

            var isKills = request.Type == ValHeatmapType.Kills;
            var events = await (
                from kill in this.context.ValorantGameKills
                join participation in participations
                    on new { kill.MatchId, Puuid = isKills ? kill.KillerPuuid : kill.VictimPuuid } equals new { participation.MatchId, Puuid = (string?)participation.Puuid }
                join round in this.context.ValorantGameRounds
                    on new { kill.MatchId, kill.RoundNumber } equals new { round.MatchId, round.RoundNumber } into rounds
                from round in rounds.DefaultIfEmpty()
                where kill.VictimPuuid != kill.KillerPuuid
                    && (isKills ? kill.KillerX != null : kill.VictimX != null)
                select new
                {
                    kill.MatchId,
                    kill.RoundNumber,
                    X = isKills ? kill.KillerX!.Value : kill.VictimX!.Value,
                    Y = isKills ? kill.KillerY!.Value : kill.VictimY!.Value,
                    Zone = isKills ? kill.KillerZone : kill.VictimZone,
                    kill.WeaponName,
                    kill.DistanceMeters,
                    participation.TeamId,
                    AttackingTeam = round != null ? round.AttackingTeam : null,
                    participation.Game.MapId,
                })
                .ToListAsync(cancellationToken);

            var mapId = events.Select(x => x.MapId).FirstOrDefault(x => x != null);
            var maps = await this.mapService.GetMapsById(cancellationToken);
            var map = mapId is not null ? maps.GetValueOrDefault(mapId) : null;

            var points = events
                .Select(x => new
                {
                    Event = x,
                    Side = x.AttackingTeam is null || x.TeamId is null
                        ? (ValSide?)null
                        : string.Equals(x.AttackingTeam, x.TeamId, StringComparison.OrdinalIgnoreCase) ? ValSide.Attack : ValSide.Defense,
                })
                .Where(x => request.Side is null || x.Side == request.Side)
                .Select(x =>
                {
                    var minimap = ValMapZoneResolver.ToMinimap(map, x.Event.X, x.Event.Y);
                    return new ValHeatmapPointDto
                    {
                        MatchId = x.Event.MatchId,
                        RoundNumber = x.Event.RoundNumber,
                        X = x.Event.X,
                        Y = x.Event.Y,
                        MinimapX = minimap is null ? null : Math.Round(minimap.Value.X, 4),
                        MinimapY = minimap is null ? null : Math.Round(minimap.Value.Y, 4),
                        Zone = x.Event.Zone,
                        Side = x.Side,
                        WeaponName = x.Event.WeaponName,
                        DistanceMeters = x.Event.DistanceMeters,
                    };
                })
                .ToList();

            return new ValHeatmapDto
            {
                MapId = mapId,
                MapName = mapName,
                Type = request.Type,
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
                Points = points,
                Zones = points
                    .Where(x => !string.IsNullOrWhiteSpace(x.Zone))
                    .GroupBy(x => x.Zone!)
                    .Select(g => new ValZoneCountDto { Zone = g.Key, Count = g.Count(), SharePercent = Math.Round(100.0 * g.Count() / points.Count, 1) })
                    .OrderByDescending(x => x.Count)
                    .ThenBy(x => x.Zone, StringComparer.Ordinal)
                    .ToList(),
            };
        }
    }
}
