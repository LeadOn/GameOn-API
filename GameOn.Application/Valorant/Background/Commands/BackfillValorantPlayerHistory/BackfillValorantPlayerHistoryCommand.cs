// <copyright file="BackfillValorantPlayerHistoryCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.BackfillValorantPlayerHistory
{
    using MediatR;

    /// <summary>
    /// Imports a player's history stored by HenrikDev: the stored MMR history, then every stored match missing from
    /// the database. Idempotent: what the database already holds is skipped.
    /// </summary>
    public class BackfillValorantPlayerHistoryCommand : IRequest<string>
    {
        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
