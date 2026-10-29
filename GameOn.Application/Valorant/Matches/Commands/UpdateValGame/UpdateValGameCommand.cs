// <copyright file="UpdateValGameCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Commands.UpdateValGame
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// Fetches or refreshes a single Valorant match by its UUID from HenrikDev and updates it in database.
    /// </summary>
    public class UpdateValGameCommand : IRequest<ValGame?>
    {
        /// <summary>
        /// Gets or sets the Valorant match UUID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional Valorant region (defaults to the stored match region or "eu").
        /// </summary>
        public string? Region { get; set; }
    }
}
