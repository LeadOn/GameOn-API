// <copyright file="UnlinkSmurfAccountCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UnlinkSmurfAccount
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UnlinkSmurfAccountCommand class. Detaches a smurf account from the player it belongs to. The
    /// account row, its rank history and its games are kept: it simply becomes a standalone account again.
    /// </summary>
    public class UnlinkSmurfAccountCommand : IRequest<Player?>
    {
        /// <summary>
        /// Gets or sets the ID of the smurf account to detach.
        /// </summary>
        public int SmurfPlayerId { get; set; }
    }
}
