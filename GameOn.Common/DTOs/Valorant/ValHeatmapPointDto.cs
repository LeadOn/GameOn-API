// <copyright file="ValHeatmapPointDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// One kill or death on a Valorant heatmap.
    /// </summary>
    public class ValHeatmapPointDto
    {
        /// <summary>
        /// Gets or sets the match UUID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the 0-based round index.
        /// </summary>
        public int RoundNumber { get; set; }

        /// <summary>
        /// Gets or sets the X game coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y game coordinate.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the position on the minimap, 0 (left) to 1 (right), null when the map has no minimap transform.
        /// </summary>
        public double? MinimapX { get; set; }

        /// <summary>
        /// Gets or sets the position on the minimap, 0 (top) to 1 (bottom).
        /// </summary>
        public double? MinimapY { get; set; }

        /// <summary>
        /// Gets or sets the map callout (e.g. A Main).
        /// </summary>
        public string? Zone { get; set; }

        /// <summary>
        /// Gets or sets the player's side in that round, null when unknown.
        /// </summary>
        public ValSide? Side { get; set; }

        /// <summary>
        /// Gets or sets the weapon of the kill.
        /// </summary>
        public string? WeaponName { get; set; }

        /// <summary>
        /// Gets or sets the distance between killer and victim, in meters.
        /// </summary>
        public double? DistanceMeters { get; set; }
    }
}
