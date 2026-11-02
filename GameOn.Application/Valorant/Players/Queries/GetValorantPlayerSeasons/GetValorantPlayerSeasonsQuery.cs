// <copyright file="GetValorantPlayerSeasonsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerSeasons
{
    using System.Collections.Generic;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// Gets a player's end-of-act Valorant results, oldest act first.
    /// </summary>
    public class GetValorantPlayerSeasonsQuery : IRequest<List<ValorantSeasonResult>>
    {
        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
