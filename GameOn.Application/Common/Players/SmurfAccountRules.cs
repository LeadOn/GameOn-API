// <copyright file="SmurfAccountRules.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Rules deciding which accounts may be linked as smurfs, shared by the League of Legends and Valorant
    /// link commands: <see cref="Player.PrimaryPlayerId"/> is a single field for every game, so both must
    /// refuse exactly the same links.
    /// </summary>
    public static class SmurfAccountRules
    {
        /// <summary>
        /// No chains: a smurf of a smurf would make every read that rolls up to
        /// <see cref="Player.PrimaryPlayerId"/> resolve to an account instead of a person, which the
        /// single-level rollup doesn't handle.
        /// </summary>
        /// <param name="primaryPlayer">The player the smurf would be attached to.</param>
        /// <returns>True if the player can hold smurfs.</returns>
        public static bool CanHoldSmurfs(Player primaryPlayer)
        {
            return primaryPlayer.PrimaryPlayerId is null;
        }

        /// <summary>
        /// Only rows that are free to be claimed can be attached. A registered player (they can log in),
        /// an account that holds smurfs of its own, and one already attached elsewhere are all refused --
        /// silently swallowing a real member's stats into someone else's profile is far worse than making
        /// the admin unlink first.
        /// </summary>
        /// <param name="context">DbContext.</param>
        /// <param name="existingAccount">Known account matching the requested Riot ID.</param>
        /// <param name="primaryPlayer">The player the smurf would be attached to.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>True if <paramref name="existingAccount"/> can be attached to <paramref name="primaryPlayer"/>.</returns>
        public static async Task<bool> IsAvailableAsSmurf(IApplicationDbContext context, Player existingAccount, Player primaryPlayer, CancellationToken cancellationToken)
        {
            var holdsSmurfs = await context.Players.AnyAsync(x => x.PrimaryPlayerId == existingAccount.Id, cancellationToken);

            return existingAccount.Id != primaryPlayer.Id
                && existingAccount.KeycloakId is null
                && !holdsSmurfs
                && (existingAccount.PrimaryPlayerId is null || existingAccount.PrimaryPlayerId == primaryPlayer.Id);
        }
    }
}
