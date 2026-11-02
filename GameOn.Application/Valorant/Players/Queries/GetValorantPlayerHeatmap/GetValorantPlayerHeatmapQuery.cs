// <copyright file="GetValorantPlayerHeatmapQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerHeatmap
{
    using System.Collections.Generic;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.Valorant;
    using MediatR;

    /// <summary>
    /// Gets where a player got their kills, or died, on one map.
    /// </summary>
    public class GetValorantPlayerHeatmapQuery : IRequest<ValHeatmapDto>
    {
        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the map name (e.g. Ascent).
        /// </summary>
        public string MapName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets what to show.
        /// </summary>
        public ValHeatmapType Type { get; set; } = ValHeatmapType.Kills;

        /// <summary>
        /// Gets or sets the side to restrict to, null for both.
        /// </summary>
        public ValSide? Side { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        public LoLStatsPeriod Period { get; set; } = LoLStatsPeriod.AllTime;

        /// <summary>
        /// Gets or sets the queues to restrict to, null for every counted queue.
        /// </summary>
        public List<string>? Queues { get; set; }
    }
}
