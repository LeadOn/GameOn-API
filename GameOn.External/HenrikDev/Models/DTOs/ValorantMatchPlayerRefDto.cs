// <copyright file="ValorantMatchPlayerRefDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Lightweight player reference used inside HenrikDev v4 match metadata, rounds, and kills.
    /// </summary>
    public class ValorantMatchPlayerRefDto
    {
        /// <summary>
        /// Gets or sets the player's Valorant PUUID.
        /// </summary>
        [JsonProperty("puuid")]
        public string? Puuid { get; set; }

        /// <summary>
        /// Gets or sets the player's Riot ID game name.
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the player's Riot ID tag line.
        /// </summary>
        [JsonProperty("tag")]
        public string? Tag { get; set; }

        /// <summary>
        /// Gets or sets the player's team identifier ("Red", "Blue", or a PUUID in free-for-all modes).
        /// </summary>
        [JsonProperty("team")]
        public string? Team { get; set; }
    }
}
