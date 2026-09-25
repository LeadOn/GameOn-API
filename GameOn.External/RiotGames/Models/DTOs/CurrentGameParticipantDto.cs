// <copyright file="CurrentGameParticipantDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// CurrentGameParticipantDto class. Mirrors spectator-v5's <c>CurrentGameParticipant</c>: one player of a
    /// game in progress. Note what isn't there: no position, since Riot only assigns one once the game is
    /// over (match-v5's <c>teamPosition</c>).
    /// </summary>
    public class CurrentGameParticipantDto
    {
        /// <summary>
        /// Gets or sets the player's PUUID. Null for a bot.
        /// </summary>
        [JsonProperty("puuid")]
        public string? Puuid { get; set; }

        /// <summary>
        /// Gets or sets the player's Riot ID (name#tag).
        /// </summary>
        [JsonProperty("riotId")]
        public string? RiotId { get; set; }

        /// <summary>
        /// Gets or sets the champion ID. Only the ID: the name is resolved from the champion referential.
        /// </summary>
        [JsonProperty("championId")]
        public long ChampionId { get; set; }

        /// <summary>
        /// Gets or sets the team: 100 for blue, 200 for red.
        /// </summary>
        [JsonProperty("teamId")]
        public long TeamId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this participant is a bot.
        /// </summary>
        [JsonProperty("bot")]
        public bool Bot { get; set; }

        /// <summary>
        /// Gets or sets the first summoner spell ID.
        /// </summary>
        [JsonProperty("spell1Id")]
        public long Spell1Id { get; set; }

        /// <summary>
        /// Gets or sets the second summoner spell ID.
        /// </summary>
        [JsonProperty("spell2Id")]
        public long Spell2Id { get; set; }

        /// <summary>
        /// Gets or sets the profile icon ID.
        /// </summary>
        [JsonProperty("profileIconId")]
        public long ProfileIconId { get; set; }
    }
}
