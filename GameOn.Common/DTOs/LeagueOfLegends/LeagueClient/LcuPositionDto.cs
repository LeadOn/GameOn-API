// <copyright file="LcuPositionDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    /// <summary>
    /// LcuPositionDto class. Map coordinates, as served by the League client.
    /// </summary>
    public class LcuPositionDto
    {
        /// <summary>
        /// Gets or sets X coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets Y coordinate.
        /// </summary>
        public int Y { get; set; }
    }
}
