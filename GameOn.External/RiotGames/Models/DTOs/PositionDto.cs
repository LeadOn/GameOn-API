// <copyright file="PositionDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// PositionDto class.
    /// </summary>
    public class PositionDto
    {
        /// <summary>
        /// Gets or sets X coordinate.
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets Y coordinate.
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }
    }
}
