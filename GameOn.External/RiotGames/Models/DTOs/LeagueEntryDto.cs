// <copyright file="LeagueEntryDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// LeagueEntryDto class.
    /// </summary>
    public class LeagueEntryDto
    {
        /// <summary>
        /// Gets or sets League ID.
        /// </summary>
        [JsonProperty("leagueId")]
        public string LeagueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Summoner ID.
        /// </summary>
        [JsonProperty("summonerId")]
        public string SummonerId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Queue Type.
        /// </summary>
        [JsonProperty("queueType")]
        public string QueueType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Tier.
        /// </summary>
        [JsonProperty("tier")]
        public string Tier { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Rank.
        /// </summary>
        [JsonProperty("rank")]
        public string Rank { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets leaguePoints.
        /// </summary>
        [JsonProperty("leaguePoints")]
        public int LeaguePoints { get; set; }

        /// <summary>
        /// Gets or sets wins.
        /// </summary>
        [JsonProperty("wins")]
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets losses.
        /// </summary>
        [JsonProperty("losses")]
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a player is on Hot streak or not.
        /// </summary>
        [JsonProperty("hotstreak")]
        public bool HotStreak { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a player is a veteran or not.
        /// </summary>
        [JsonProperty("veteran")]
        public bool Veteran { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a player is a fresh blood or not.
        /// </summary>
        [JsonProperty("freshBlood")]
        public bool FreshBlood { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a player is inactive or not.
        /// </summary>
        [JsonProperty("inactive")]
        public bool Inactive { get; set; } = false;
    }
}
