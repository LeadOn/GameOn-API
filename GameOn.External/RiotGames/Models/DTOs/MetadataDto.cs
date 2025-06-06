﻿// <copyright file="MetadataDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// MetadataDto class.
    /// </summary>
    public class MetadataDto
    {
        /// <summary>
        /// Gets or sets data version.
        /// </summary>
        [JsonProperty("dataVersion")]
        public string DataVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        [JsonProperty("matchId")]
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets participants (PUUID).
        /// </summary>
        [JsonProperty("participants")]
        public List<string> Participants { get; set; } = new List<string>();
    }
}
