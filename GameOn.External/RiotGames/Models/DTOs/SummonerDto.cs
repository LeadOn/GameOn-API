// <copyright file="SummonerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// SummonerDto class.
    /// </summary>
    public class SummonerDto
    {
        /// <summary>
        /// Gets or sets PUUID.
        /// </summary>
        [JsonProperty("puuid")]
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Account ID.
        /// </summary>
        [JsonProperty("accountId")]
        public string AccountId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Profile Icon ID.
        /// </summary>
        [JsonProperty("profileIconId")]
        public int ProfileIconId { get; set; }

        /// <summary>
        /// Gets or sets Revision Date.
        /// </summary>
        [JsonProperty("revisionDate")]
        public long RevisionDate { get; set; }

        /// <summary>
        /// Gets or sets Summoner ID.
        /// </summary>
        [JsonProperty("id")]
        public string SummonerId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets summoner levelDate.
        /// </summary>
        [JsonProperty("summonerLevel")]
        public long SummonerLevel { get; set; }
    }
}
