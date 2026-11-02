// <copyright file="RecomputeValGameStatsCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.RecomputeValGameStats
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Services;
    using GameOn.Common.Interfaces;
    using GameOn.External.ValorantApi.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="RecomputeValGameStatsCommand"/> handler. Each match runs in its own DI scope so that the
    /// DbContext never tracks more than one match.
    /// </summary>
    public class RecomputeValGameStatsCommandHandler : IRequestHandler<RecomputeValGameStatsCommand, int>
    {
        private readonly IApplicationDbContext context;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IValorantMapService mapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecomputeValGameStatsCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="scopeFactory">Scope factory, injected.</param>
        /// <param name="mapService">Valorant map reference data, injected.</param>
        public RecomputeValGameStatsCommandHandler(IApplicationDbContext context, IServiceScopeFactory scopeFactory, IValorantMapService mapService)
        {
            this.context = context;
            this.scopeFactory = scopeFactory;
            this.mapService = mapService;
        }

        /// <inheritdoc />
        public async Task<int> Handle(RecomputeValGameStatsCommand request, CancellationToken cancellationToken)
        {
            var query = this.context.ValorantGames.AsQueryable();

            if (request.Since is { } since)
            {
                query = query.Where(x => x.GameStart >= since);
            }

            var matchIds = await query.Select(x => x.MatchId).ToListAsync(cancellationToken);
            var maps = await this.mapService.GetMapsById(cancellationToken);

            foreach (var matchId in matchIds)
            {
                using var scope = this.scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                var game = await db.ValorantGames.FirstAsync(x => x.MatchId == matchId, cancellationToken);
                var participants = await db.ValorantGameParticipants.Include(x => x.Stats).Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                var rounds = await db.ValorantGameRounds.Where(x => x.MatchId == matchId).OrderBy(x => x.RoundNumber).ToListAsync(cancellationToken);
                var kills = await db.ValorantGameKills.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                var roundPlayerStats = await db.ValorantGameRoundPlayerStats.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                var map = game.MapId is not null ? maps.GetValueOrDefault(game.MapId) : null;

                foreach (var kill in kills)
                {
                    kill.VictimZone = ValMapZoneResolver.Resolve(map, kill.VictimX, kill.VictimY) ?? kill.VictimZone;
                    kill.KillerZone = ValMapZoneResolver.Resolve(map, kill.KillerX, kill.KillerY) ?? kill.KillerZone;

                    if (kill.KillerX is not null && kill.VictimX is not null)
                    {
                        var dx = kill.KillerX.Value - kill.VictimX.Value;
                        var dy = kill.KillerY!.Value - kill.VictimY!.Value;
                        kill.DistanceMeters = Math.Round(Math.Sqrt((dx * (double)dx) + (dy * (double)dy)) / 100.0, 1);
                    }
                }

                foreach (var round in rounds)
                {
                    round.PlantZone = ValMapZoneResolver.Resolve(map, round.PlantX, round.PlantY) ?? round.PlantZone;
                }

                ValGameAdvancedStatCalculator.ResolveSides(rounds, participants, game.QueueId);
                ValGameParticipantStatCalculator.ComputeAll(participants, rounds, kills);
                ValGameAdvancedStatCalculator.ComputeAll(participants, rounds, kills, roundPlayerStats, game.QueueId);

                var (mvp, ace) = ValGameParticipantStatCalculator.DetermineMvpAndAce(participants, game.WinningTeamId, game.IsRemake);
                game.MvpParticipantId = mvp;
                game.AceParticipantId = ace;

                await db.SaveChangesAsync(cancellationToken);
            }

            return matchIds.Count;
        }
    }
}
