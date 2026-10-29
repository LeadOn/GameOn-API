// <copyright file="ValorantMatchPlayerStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Aggregate combat statistics for a player inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchPlayerStatsDto
    {
        /// <summary>
        /// Gets or sets the total combat score across all rounds.
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the total kills.
        /// </summary>
        [JsonProperty("kills")]
        public int Kills { get; set; }

        /// <summary>
        /// Gets or sets the total deaths.
        /// </summary>
        [JsonProperty("deaths")]
        public int Deaths { get; set; }

        /// <summary>
        /// Gets or sets the total assists.
        /// </summary>
        [JsonProperty("assists")]
        public int Assists { get; set; }

        /// <summary>
        /// Gets or sets the total headshot hits.
        /// </summary>
        [JsonProperty("headshots")]
        public int Headshots { get; set; }

        /// <summary>
        /// Gets or sets the total bodyshot hits.
        /// </summary>
        [JsonProperty("bodyshots")]
        public int Bodyshots { get; set; }

        /// <summary>
        /// Gets or sets the total legshot hits.
        /// </summary>
        [JsonProperty("legshots")]
        public int Legshots { get; set; }

        /// <summary>
        /// Gets or sets the total damage dealt and received.
        /// </summary>
        [JsonProperty("damage")]
        public ValorantMatchDamageDto? Damage { get; set; }
    }
}
