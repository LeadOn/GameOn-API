// <copyright file="ValorantApiVectorDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.ValorantApi.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Game coordinates on <c>valorant-api.com</c>.
    /// </summary>
    public class ValorantApiVectorDto
    {
        /// <summary>
        /// Gets or sets the X game coordinate.
        /// </summary>
        [JsonProperty("x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y game coordinate.
        /// </summary>
        [JsonProperty("y")]
        public double Y { get; set; }
    }
}
