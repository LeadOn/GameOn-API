// <copyright file="ValorantMatchBloomlineDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Agent mastery cosmetics (bloomline) equipped by a player in a match.
    /// </summary>
    public class ValorantMatchBloomlineDto
    {
        /// <summary>
        /// Gets or sets the bloomline level.
        /// </summary>
        [JsonProperty("level")]
        public int? Level { get; set; }

        /// <summary>
        /// Gets or sets the background UUID.
        /// </summary>
        [JsonProperty("plank_background_id")]
        public string? PlankBackgroundId { get; set; }

        /// <summary>
        /// Gets or sets the hype UUID.
        /// </summary>
        [JsonProperty("hype_id")]
        public string? HypeId { get; set; }

        /// <summary>
        /// Gets or sets the clip UUID.
        /// </summary>
        [JsonProperty("clip_id")]
        public string? ClipId { get; set; }
    }
}
