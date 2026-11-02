// <copyright file="GetValorantPlayerAccoladesQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerAccolades
{
    using System.Collections.Generic;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// Gets a player's Valorant accolade summaries: the all-time rows (no act) first, then each act.
    /// </summary>
    public class GetValorantPlayerAccoladesQuery : IRequest<List<ValorantAccoladeSummary>>
    {
        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
