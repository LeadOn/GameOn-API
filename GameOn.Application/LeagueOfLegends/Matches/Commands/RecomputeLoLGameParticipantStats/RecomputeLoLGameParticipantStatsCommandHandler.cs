// <copyright file="RecomputeLoLGameParticipantStatsCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.RecomputeLoLGameParticipantStats
{
    using GameOn.Application.LeagueOfLegends.Matches.Services;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// RecomputeLoLGameParticipantStatsCommandHandler class.
    /// </summary>
    public class RecomputeLoLGameParticipantStatsCommandHandler : IRequestHandler<RecomputeLoLGameParticipantStatsCommand, int>
    {
        private const int BatchSize = 200;

        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecomputeLoLGameParticipantStatsCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public RecomputeLoLGameParticipantStatsCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<int> Handle(RecomputeLoLGameParticipantStatsCommand request, CancellationToken cancellationToken)
        {
            var matchIds = await this.context.LeagueOfLegendsGames.Select(x => x.MatchId).ToListAsync(cancellationToken);
            var totalUpdated = 0;

            foreach (var batch in matchIds.Chunk(BatchSize))
            {
                totalUpdated += await this.ProcessBatchAsync(batch, cancellationToken);
            }

            return totalUpdated;
        }

        private async Task<int> ProcessBatchAsync(string[] matchIds, CancellationToken cancellationToken)
        {
            var participants = await this.context.LeagueOfLegendsGameParticipants
                .Include(x => x.Stats)
                .Where(x => matchIds.Contains(x.MatchId))
                .ToListAsync(cancellationToken);

            if (participants.Count == 0)
            {
                return 0;
            }

            var lastFrameTimestamps = await this.context.LeagueOfLegendsGameTimelineFrames
                .Where(f => matchIds.Contains(f.MatchId))
                .GroupBy(f => f.MatchId)
                .Select(g => new { MatchId = g.Key, Timestamp = g.Max(f => f.Timestamp) })
                .ToDictionaryAsync(x => x.MatchId, x => x.Timestamp, cancellationToken);

            // Last timeline frame of each match, for every participant (cumulative stats = end of game values).
            var lastFrameParticipants = await this.context.LeagueOfLegendsGameTimelineFrameParticipants
                .Where(x => matchIds.Contains(x.TimelineFrame.MatchId)
                    && x.TimelineFrame.Timestamp == x.TimelineFrame.Game.LoLGameTimelineFrames.Max(f => f.Timestamp))
                .Select(x => new
                {
                    x.TimelineFrame.MatchId,
                    x.ParticipantPUUID,
                    x.MinionsKilled,
                    x.JungleMinionsKilled,
                    x.TotalGold,
                    x.TotalDamageDoneToChampions,
                    x.TotalDamageTaken,
                })
                .ToListAsync(cancellationToken);

            // Bots can share the same placeholder PUUID ("BOT") within a match: grouped with .First()
            // instead of a plain ToDictionary to avoid duplicate (MatchId, ParticipantPUUID) keys.
            var lastFrameLookup = lastFrameParticipants
                .GroupBy(x => (x.MatchId, x.ParticipantPUUID))
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var frame = g.First();
                        return new LoLGameTimelineFrameParticipant
                        {
                            MinionsKilled = frame.MinionsKilled,
                            JungleMinionsKilled = frame.JungleMinionsKilled,
                            TotalGold = frame.TotalGold,
                            TotalDamageDoneToChampions = frame.TotalDamageDoneToChampions,
                            TotalDamageTaken = frame.TotalDamageTaken,
                        };
                    });

            var wardsPlacedLookup = (await this.context.LeagueOfLegendsGameTimelineEvents
                .Where(x => matchIds.Contains(x.MatchId) && x.EventType == "WARD_PLACED" && x.CreatorPUUID != null)
                .GroupBy(x => new { x.MatchId, x.CreatorPUUID })
                .Select(g => new { g.Key.MatchId, g.Key.CreatorPUUID, Count = g.Count() })
                .ToListAsync(cancellationToken))
                .ToDictionary(x => (x.MatchId, x.CreatorPUUID!), x => x.Count);

            var wardsKilledLookup = (await this.context.LeagueOfLegendsGameTimelineEvents
                .Where(x => matchIds.Contains(x.MatchId) && x.EventType == "WARD_KILL" && x.KillerPUUID != null)
                .GroupBy(x => new { x.MatchId, x.KillerPUUID })
                .Select(g => new { g.Key.MatchId, g.Key.KillerPUUID, Count = g.Count() })
                .ToListAsync(cancellationToken))
                .ToDictionary(x => (x.MatchId, x.KillerPUUID!), x => x.Count);

            foreach (var matchParticipants in participants.GroupBy(x => x.MatchId))
            {
                var lastFrameTimestampMs = lastFrameTimestamps.GetValueOrDefault(matchParticipants.Key);

                foreach (var teamParticipants in matchParticipants.GroupBy(x => x.TeamId))
                {
                    var teamKills = teamParticipants.Sum(x => x.Kills);

                    foreach (var participant in teamParticipants)
                    {
                        lastFrameLookup.TryGetValue((participant.MatchId, participant.Puuid), out var lastFrame);

                        participant.Stats = LoLGameParticipantStatCalculator.Compute(
                            participant,
                            teamKills,
                            lastFrame,
                            lastFrameTimestampMs,
                            wardsPlacedLookup.GetValueOrDefault((participant.MatchId, participant.Puuid)),
                            wardsKilledLookup.GetValueOrDefault((participant.MatchId, participant.Puuid)),
                            participant.Stats);
                    }
                }
            }

            await this.context.SaveChangesAsync(cancellationToken);

            return participants.Count;
        }
    }
}
