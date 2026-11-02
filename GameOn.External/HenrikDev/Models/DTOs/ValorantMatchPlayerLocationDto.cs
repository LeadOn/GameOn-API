// <copyright file="ValorantMatchPlayerLocationDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Where one player stood, and where they looked, at a kill, plant or defuse.
    /// </summary>
    public class ValorantMatchPlayerLocationDto
    {
        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        [JsonProperty("player")]
        public ValorantMatchPlayerRefDto? Player { get; set; }

        /// <summary>
        /// Gets or sets the view direction, in radians.
        /// </summary>
        [JsonProperty("view_radians")]
        public double ViewRadians { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        [JsonProperty("location")]
        public ValorantMatchLocationDto? Location { get; set; }
    }
}
