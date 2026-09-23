// <copyright file="RecomputeLoLGameRankChangesCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.RecomputeLoLGameRankChanges
{
    using GameOn.Application.LeagueOfLegends.Matches.Services;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// RecomputeLoLGameRankChangesCommandHandler class.
    /// </summary>
    /// <remarks>
    /// The stored LP changes are a pure function of the snapshots and games in database, so this is a
    /// sync, not an append: every participation in range ends up holding exactly what
    /// <see cref="LoLGameRankChangeCalculator"/> says, whatever it held before. That is what lets it
    /// run on every refresh, and fix a game whose value became ambiguous since (a game imported late,
    /// landing between the same two snapshots as one already attributed).
    /// </remarks>
    public class RecomputeLoLGameRankChangesCommandHandler : IRequestHandler<RecomputeLoLGameRankChangesCommand, RecomputeLoLGameRankChangesResultDto>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecomputeLoLGameRankChangesCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public RecomputeLoLGameRankChangesCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<RecomputeLoLGameRankChangesResultDto> Handle(RecomputeLoLGameRankChangesCommand request, CancellationToken cancellationToken)
        {
            var result = new RecomputeLoLGameRankChangesResultDto();

            // Only accounts with snapshots can have an LP change: the others were never refreshed.
            var playerIds = request.PlayerId is not null
                ? new List<int> { request.PlayerId.Value }
                : await this.context.LeagueOfLegendsRankHistory.Select(x => x.PlayerId).Distinct().ToListAsync(cancellationToken);

            foreach (var playerId in playerIds)
            {
                foreach (var queue in LoLGameRankChangeCalculator.RankedQueueTypes)
                {
                    await this.RecomputeQueue(playerId, queue.Key, queue.Value, request.Since, request.Until, result, cancellationToken);
                }

                await this.context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        private static bool HasSameReading(LoLGameParticipantRankChange stored, LoLGameParticipantRankChange computed)
        {
            return stored.LeaguePointsChange == computed.LeaguePointsChange
                && stored.TierBefore == computed.TierBefore
                && stored.RankBefore == computed.RankBefore
                && stored.LeaguePointsBefore == computed.LeaguePointsBefore
                && stored.TierAfter == computed.TierAfter
                && stored.RankAfter == computed.RankAfter
                && stored.LeaguePointsAfter == computed.LeaguePointsAfter;
        }

        private async Task RecomputeQueue(int playerId, int queueId, string queueType, DateTime? since, DateTime? until, RecomputeLoLGameRankChangesResultDto result, CancellationToken cancellationToken)
        {
            var snapshotsQuery = this.context.LeagueOfLegendsRankHistory
                .AsNoTracking()
                .Where(x => x.PlayerId == playerId && x.QueueType == queueType);

            // A game's LP sits between the last snapshot before it and the first one after it, so the
            // requested bounds are widened out to those two snapshots. Every game ending in between then
            // has both sides of its window loaded, along with every other game that could share it. No
            // snapshot beyond a bound means no game past it can be attributed, so the bound stays as asked.
            var lowerSnapshot = since is null
                ? null
                : await snapshotsQuery
                    .Where(x => x.CreatedOn < since)
                    .OrderByDescending(x => x.CreatedOn)
                    .ThenByDescending(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);

            var upperSnapshot = until is null
                ? null
                : await snapshotsQuery
                    .Where(x => x.CreatedOn > until)
                    .OrderBy(x => x.CreatedOn)
                    .ThenBy(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);

            var lowerBound = lowerSnapshot?.CreatedOn ?? since;
            var upperBound = upperSnapshot?.CreatedOn ?? until;

            var participationsQuery = this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.PlayerId == playerId && x.Game.QueueId == queueId);
            var storedQuery = this.context.LeagueOfLegendsGameParticipantRankChanges
                .Where(x => x.Participant.PlayerId == playerId && x.Participant.Game.QueueId == queueId);

            if (lowerBound is not null)
            {
                snapshotsQuery = snapshotsQuery.Where(x => x.CreatedOn >= lowerBound);
                participationsQuery = participationsQuery.Where(x => x.Game.GameEnd > lowerBound);
                storedQuery = storedQuery.Where(x => x.Participant.Game.GameEnd > lowerBound);
            }

            if (upperBound is not null)
            {
                snapshotsQuery = snapshotsQuery.Where(x => x.CreatedOn <= upperBound);
                participationsQuery = participationsQuery.Where(x => x.Game.GameEnd <= upperBound);
                storedQuery = storedQuery.Where(x => x.Participant.Game.GameEnd <= upperBound);
            }

            var snapshots = await snapshotsQuery.ToListAsync(cancellationToken);
            var participations = await participationsQuery
                .Select(x => new { x.Id, x.MatchId, x.Game.GameEnd, x.Game.IsRemake, x.Win })
                .ToListAsync(cancellationToken);
            var stored = await storedQuery.ToDictionaryAsync(x => x.LoLGameParticipantId, cancellationToken);

            var computed = LoLGameRankChangeCalculator.Compute(
                snapshots,
                participations.Where(x => !x.IsRemake).Select(x => (x.MatchId, x.GameEnd, x.Win)));

            foreach (var participation in participations)
            {
                result.ParticipationsScanned++;
                computed.TryGetValue(participation.MatchId, out var change);
                stored.TryGetValue(participation.Id, out var storedChange);

                if (change is null)
                {
                    if (storedChange is not null)
                    {
                        this.context.LeagueOfLegendsGameParticipantRankChanges.Remove(storedChange);
                        result.Removed++;
                    }

                    continue;
                }

                result.ParticipationsWithRankChange++;

                if (storedChange is null)
                {
                    change.LoLGameParticipantId = participation.Id;
                    this.context.LeagueOfLegendsGameParticipantRankChanges.Add(change);
                    result.Created++;
                }
                else if (!HasSameReading(storedChange, change))
                {
                    // Left untouched when nothing moved, ComputedOn included: this runs on every refresh,
                    // and rewriting identical rows every 20 minutes would only be noise.
                    storedChange.LeaguePointsChange = change.LeaguePointsChange;
                    storedChange.TierBefore = change.TierBefore;
                    storedChange.RankBefore = change.RankBefore;
                    storedChange.LeaguePointsBefore = change.LeaguePointsBefore;
                    storedChange.TierAfter = change.TierAfter;
                    storedChange.RankAfter = change.RankAfter;
                    storedChange.LeaguePointsAfter = change.LeaguePointsAfter;
                    storedChange.ComputedOn = DateTime.UtcNow;
                    result.Updated++;
                }
            }
        }
    }
}
