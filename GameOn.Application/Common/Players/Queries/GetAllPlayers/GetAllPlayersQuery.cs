// <copyright file="GetAllPlayersQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Queries.GetAllPlayers
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllPlayersQuery class.
    /// </summary>
    public class GetAllPlayersQuery : IRequest<IEnumerable<Player>>
    {
        /// <summary>
        /// Gets or sets a value indicating whether a player is archived or not.
        /// </summary>
        public bool Archived { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether smurf accounts are listed alongside the members they
        /// belong to. On by default, so that linking an account never makes it silently vanish from a
        /// list: entries carry <see cref="Player.PrimaryPlayerId"/> and the caller decides. Turn it off
        /// for pickers that must offer people rather than accounts (teams, tournament subscriptions).
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;
    }
}
