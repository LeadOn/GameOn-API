// <copyright file="ValMapTransformDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// How to project a map's game coordinates onto its minimap (0 to 1 on both axes): <c>minimapX = gameY * XMultiplier + XScalarToAdd</c>
    /// and <c>minimapY = gameX * YMultiplier + YScalarToAdd</c>. Valorant swaps the axes. Same values as <c>valorant-api.com/v1/maps</c>.
    /// </summary>
    public class ValMapTransformDto
    {
        /// <summary>
        /// Gets or sets the minimap image URL.
        /// </summary>
        public string? MinimapUrl { get; set; }

        /// <summary>
        /// Gets or sets the factor applied to the game Y to get the minimap X.
        /// </summary>
        public double XMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the factor applied to the game X to get the minimap Y.
        /// </summary>
        public double YMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the offset added to the minimap X.
        /// </summary>
        public double XScalarToAdd { get; set; }

        /// <summary>
        /// Gets or sets the offset added to the minimap Y.
        /// </summary>
        public double YScalarToAdd { get; set; }
    }
}
