// <copyright file="ValorantAccountDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Valorant account payload returned by <c>GET /valorant/v2/account/{name}/{tag}</c> and
    /// <c>GET /valorant/v2/by-puuid/account/{puuid}</c>.
    /// </summary>
    public class ValorantAccountDto
    {
        /// <summary>
        /// Gets or sets the player's Valorant PUUID (36-char UUID).
        /// </summary>
        [JsonProperty("puuid")]
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the player's Valorant region affinity (e.g. eu, na, ap, kr, latam, br).
        /// </summary>
        [JsonProperty("region")]
        public string? Region { get; set; }

        /// <summary>
        /// Gets or sets the player's Valorant account level.
        /// </summary>
        [JsonProperty("account_level")]
        public int AccountLevel { get; set; }

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
        /// Gets or sets the equipped player card UUID.
        /// </summary>
        [JsonProperty("card")]
        public string? Card { get; set; }

        /// <summary>
        /// Gets or sets the equipped player title UUID.
        /// </summary>
        [JsonProperty("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the platforms the account has played on (e.g. PC, CONSOLE).
        /// </summary>
        [JsonProperty("platforms")]
        public List<string> Platforms { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the ISO timestamp at which the account was last updated on HenrikDev's side.
        /// </summary>
        [JsonProperty("updated_at")]
        public string? UpdatedAt { get; set; }
    }
}