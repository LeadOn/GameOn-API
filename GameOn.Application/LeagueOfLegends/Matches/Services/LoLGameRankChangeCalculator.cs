// <copyright file="LoLGameRankChangeCalculator.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Services
{
    using GameOn.Application.LeagueOfLegends.Summoners.Services;
    using GameOn.Domain;

    /// <summary>
    /// Pins league point changes on individual ranked games. Riot exposes no per-game LP: match-v5 knows
    /// nothing about ranks and league-v4 only gives the current reading, which the rank refresh stores
    /// as <see cref="LeagueOfLegendsRankHistory"/> snapshots. Two consecutive snapshots of a queue whose
    /// win + loss counters are one game apart bracket exactly one game, and their LP difference is that
    /// game's. Everything else (several games between two snapshots, a season reset, a counter that
    /// disagrees with the game's result) is left unattributed rather than guessed: a missing value is
    /// harmless on a match card, a wrong one is not.
    /// </summary>
    public static class LoLGameRankChangeCalculator
    {
        /// <summary>
        /// The two ranked queues, keyed by their match-v5 queue ID. league-v4 names the same queues with
        /// its own strings, the two Riot APIs share no identifier.
        /// </summary>
        public static readonly IReadOnlyDictionary<int, string> RankedQueueTypes = new Dictionary<int, string>
        {
            { 420, "RANKED_SOLO_5x5" },
            { 440, "RANKED_FLEX_SR" },
        };

        /// <summary>
        /// Pins LP changes on the games of one player in one ranked queue.
        /// </summary>
        /// <param name="snapshots">The player's rank snapshots for that queue, in any order.</param>
        /// <param name="games">The player's games in the matching match-v5 queue, remakes excluded: a
        /// remake never moves the win/loss counters of the players who stayed.</param>
        /// <returns>The LP change of every game that could be pinned, keyed by match ID. The
        /// participant ID is left for the caller to set.</returns>
        public static Dictionary<string, LoLGameParticipantRankChange> Compute(
            IEnumerable<LeagueOfLegendsRankHistory> snapshots,
            IEnumerable<(string MatchId, DateTime GameEnd, bool Win)> games)
        {
            var result = new Dictionary<string, LoLGameParticipantRankChange>();
            var orderedSnapshots = snapshots.OrderBy(x => x.CreatedOn).ThenBy(x => x.Id).ToList();
            var orderedGames = games.OrderBy(x => x.GameEnd).ToList();

            for (var i = 1; i < orderedSnapshots.Count; i++)
            {
                var before = orderedSnapshots[i - 1];
                var after = orderedSnapshots[i];
                var winsGained = after.Wins - before.Wins;
                var lossesGained = after.Losses - before.Losses;

                // Exactly one game on Riot's counters. More than one and the LP difference is a sum that
                // can't be split fairly; a negative difference is a season reset.
                if (!(winsGained == 1 && lossesGained == 0) && !(winsGained == 0 && lossesGained == 1))
                {
                    continue;
                }

                // Exactly one game on our side too. The snapshot is only written once league-v4 has moved,
                // so the game it reflects ended after the previous snapshot and before this one. Two
                // candidates means one of them isn't the one Riot counted, and there's no telling which.
                var candidates = orderedGames
                    .Where(x => x.GameEnd > before.CreatedOn && x.GameEnd <= after.CreatedOn)
                    .ToList();

                if (candidates.Count != 1)
                {
                    continue;
                }

                var game = candidates[0];

                if (game.Win != (winsGained == 1))
                {
                    continue;
                }

                var normalizedBefore = LoLRankScaleCalculator.NormalizedLp(before.Tier, before.Rank, before.LeaguePoints);
                var normalizedAfter = LoLRankScaleCalculator.NormalizedLp(after.Tier, after.Rank, after.LeaguePoints);

                if (normalizedBefore is null || normalizedAfter is null)
                {
                    continue;
                }

                var change = normalizedAfter.Value - normalizedBefore.Value;

                // A win always earns LP, and a loss never does (it can cost nothing at 0 LP, when demotion
                // protection holds). The other way around, something else moved the LP within the same
                // window -- a champion select dodge, decay -- and the difference isn't the game's alone.
                if (game.Win ? change <= 0 : change > 0)
                {
                    continue;
                }

                result[game.MatchId] = new LoLGameParticipantRankChange
                {
                    LeaguePointsChange = change,
                    TierBefore = before.Tier,
                    RankBefore = before.Rank,
                    LeaguePointsBefore = before.LeaguePoints,
                    TierAfter = after.Tier,
                    RankAfter = after.Rank,
                    LeaguePointsAfter = after.LeaguePoints,
                };
            }

            return result;
        }
    }
}
