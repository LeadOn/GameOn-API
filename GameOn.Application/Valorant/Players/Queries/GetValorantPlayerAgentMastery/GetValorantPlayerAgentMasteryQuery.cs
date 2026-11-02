// <copyright file="GetValorantPlayerAgentMasteryQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerAgentMastery
{
    using System.Collections.Generic;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// Gets a player's Valorant agent mastery, highest level first.
    /// </summary>
    public class GetValorantPlayerAgentMasteryQuery : IRequest<List<ValorantAgentMastery>>
    {
        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
