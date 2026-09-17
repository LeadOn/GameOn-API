// <copyright file="CommunityDragonChampionDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.CommunityDragon.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// CommunityDragonChampionDto class. Mirrors the champion objects served by Community Dragon's
    /// game data mirror.
    /// </summary>
    public class CommunityDragonChampionDto
    {
        /// <summary>
        /// Gets or sets champion ID. Same referential as Riot's championId.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the display name (Wukong, Nunu &amp; Willump, ...).
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the alias (MonkeyKing, Nunu, ...). This is what match-v5 returns as
        /// championName, and therefore what LoLGameParticipant.ChampionName holds everywhere else.
        /// </summary>
        [JsonProperty("alias")]
        public string? Alias { get; set; }
    }
}
