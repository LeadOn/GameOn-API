// <copyright file="PlayerQueryExtensions.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players
{
    using GameOn.Domain;

    /// <summary>
    /// Query helpers around the two flags that decide which accounts a read is about:
    /// <see cref="Player.InCrew"/>, which says whether an account counts towards the crew's numbers, and
    /// <see cref="Player.PrimaryPlayerId"/>, the link between a smurf account and
    /// the member it belongs to. That link is declarative: it says who owns the account and nothing more.
    /// Every account keeps its own rank, its own games and its own records, so no read aggregates a smurf
    /// into its owner — the only thing this offers is the ability to leave smurfs out of a list.
    /// </summary>
    public static class PlayerQueryExtensions
    {
        /// <summary>
        /// Keeps primary accounts only, dropping the smurf rows attached to them. Never applied by
        /// default: a list that silently hides rows looks like lost data. Callers that need people
        /// rather than accounts (team and tournament pickers) opt in explicitly.
        /// </summary>
        /// <param name="players">Players query.</param>
        /// <returns>The query, restricted to primary accounts.</returns>
        public static IQueryable<Player> PrimariesOnly(this IQueryable<Player> players)
        {
            return players.Where(x => x.PrimaryPlayerId == null);
        }

        /// <summary>
        /// Keeps crew accounts only. Applied by default by everything that describes the crew -- the
        /// player lists, the global stats, the home page -- because an account left out of the crew is
        /// precisely one whose numbers should stop counting. Reads about one specific account (its
        /// profile, its own match history) never apply it: the account still exists and is still
        /// refreshable on demand.
        /// </summary>
        /// <param name="players">Players query.</param>
        /// <returns>The query, restricted to crew accounts.</returns>
        public static IQueryable<Player> InCrewOnly(this IQueryable<Player> players)
        {
            return players.Where(x => x.InCrew);
        }
    }
}
