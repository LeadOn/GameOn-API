// <copyright file="UpsertValorantMatchesCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Commands.UpsertValorantMatches
{
    using System.Collections.Generic;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Models.DTOs;
    using MediatR;

    /// <summary>
    /// Persists or updates a batch of HenrikDev v4 Valorant match payloads, computing participant
    /// derived stats, MVP/ACE awards, and linking tracked players and RR changes.
    /// </summary>
    public class UpsertValorantMatchesCommand : IRequest<List<ValGame>>
    {
        /// <summary>
        /// Gets or sets the HenrikDev v4 match payloads to persist.
        /// </summary>
        public List<ValorantMatchDto> Matches { get; set; } = new List<ValorantMatchDto>();

        /// <summary>
        /// Gets or sets a value indicating whether matches already present in database should be
        /// re-imported from scratch. Defaults to false (only new matches are inserted, while existing
        /// matches have any newly available PlayerId or RR change backfilled).
        /// </summary>
        public bool OverwriteExisting { get; set; }
    }
}
