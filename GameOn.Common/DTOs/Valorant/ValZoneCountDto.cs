// <copyright file="ValZoneCountDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// Number of kills or deaths in one zone of a map.
    /// </summary>
    public class ValZoneCountDto
    {
        /// <summary>
        /// Gets or sets the map callout.
        /// </summary>
        public string Zone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of events.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the share of the events, in percent.
        /// </summary>
        public double SharePercent { get; set; }
    }
}
