// <copyright file="ValorantMatchFriendlyFireDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Friendly-fire damage counters inside a HenrikDev v4 match player behavior object.
    /// </summary>
    public class ValorantMatchFriendlyFireDto
    {
        /// <summary>
        /// Gets or sets the friendly-fire damage received from teammates.
        /// </summary>
        [JsonProperty("incoming")]
        public double? Incoming { get; set; }

        /// <summary>
        /// Gets or sets the friendly-fire damage dealt to teammates.
        /// </summary>
        [JsonProperty("outgoing")]
        public double? Outgoing { get; set; }
    }
}
