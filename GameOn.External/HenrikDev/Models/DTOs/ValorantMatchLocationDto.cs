// <copyright file="ValorantMatchLocationDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Game coordinates inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchLocationDto
    {
        /// <summary>
        /// Gets or sets the X game coordinate.
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y game coordinate.
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }
    }
}
