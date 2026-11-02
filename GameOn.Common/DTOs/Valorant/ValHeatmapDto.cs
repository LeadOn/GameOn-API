// <copyright file="ValHeatmapDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System.Collections.Generic;

    /// <summary>
    /// Positions of a player's kills or deaths on one map, ready to draw on the minimap.
    /// </summary>
    public class ValHeatmapDto
    {
        /// <summary>
        /// Gets or sets the map UUID.
        /// </summary>
        public string? MapId { get; set; }

        /// <summary>
        /// Gets or sets the map name.
        /// </summary>
        public string MapName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets what the points are.
        /// </summary>
        public ValHeatmapType Type { get; set; }

        /// <summary>
        /// Gets or sets the minimap image and transform, null when the map has none.
        /// </summary>
        public ValMapTransformDto? MapTransform { get; set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        public List<ValHeatmapPointDto> Points { get; set; } = new List<ValHeatmapPointDto>();

        /// <summary>
        /// Gets or sets the count per zone, most first.
        /// </summary>
        public List<ValZoneCountDto> Zones { get; set; } = new List<ValZoneCountDto>();
    }
}
