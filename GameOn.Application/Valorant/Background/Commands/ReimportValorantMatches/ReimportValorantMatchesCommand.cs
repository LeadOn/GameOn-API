// <copyright file="ReimportValorantMatchesCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.ReimportValorantMatches
{
    using System;
    using MediatR;

    /// <summary>
    /// Re-imports matches already in database from HenrikDev (1 call per match), to fill the columns added since
    /// they were first imported.
    /// </summary>
    public class ReimportValorantMatchesCommand : IRequest<string>
    {
        /// <summary>
        /// Gets or sets the player whose matches are re-imported, null for every match.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the oldest match start re-imported, null for no bound.
        /// </summary>
        public DateTime? Since { get; set; }
    }
}
