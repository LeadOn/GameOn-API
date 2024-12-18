// <copyright file="AccountDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// AccountDto class.
    /// </summary>
    public class AccountDto
    {
        /// <summary>
        /// Gets or sets PUUID.
        /// </summary>
        [JsonProperty("puuid")]
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Game Name.
        /// </summary>
        [JsonProperty("gameName")]
        public string? GameName { get; set; }

        /// <summary>
        /// Gets or sets Tag Line.
        /// </summary>
        [JsonProperty("tagLine")]
        public string? TagLine { get; set; }
    }
}
