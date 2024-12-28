// <copyright file="ParticipantDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// ParticipantDto class.
    /// </summary>
    public class ParticipantDto
    {
        /// <summary>
        /// Gets or sets All In Pings.
        /// </summary>
        [JsonProperty("allInPings")]
        public int AllInPings { get; set; }

        /// <summary>
        /// Gets or sets Assist me pings.
        /// </summary>
        [JsonProperty("assistMePings")]
        public int AssistMePings { get; set; }

        /// <summary>
        /// Gets or sets assists.
        /// </summary>
        [JsonProperty("assists")]
        public int Assists { get; set; }

        /// <summary>
        /// Gets or sets baron kills.
        /// </summary>
        [JsonProperty("baronKills")]
        public int BaronKills { get; set; }

        /// <summary>
        /// Gets or sets bounty level.
        /// </summary>
        [JsonProperty("bountyLevel")]
        public int BountyLevel { get; set; }

        /// <summary>
        /// Gets or sets Champ experience.
        /// </summary>
        [JsonProperty("champExperience")]
        public int ChampExperience { get; set; }

        /// <summary>
        /// Gets or sets champ level.
        /// </summary>
        [JsonProperty("champLevel")]
        public int ChampLevel { get; set; }

        /// <summary>
        /// Gets or sets champion ID.
        /// </summary>
        [JsonProperty("championId")]
        public int ChampionId { get; set; }

        /// <summary>
        /// Gets or sets championName.
        /// </summary>
        [JsonProperty("championName")]
        public string ChampionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets command Pings.
        /// </summary>
        [JsonProperty("commandPings")]
        public int CommandPings { get; set; }

        /// <summary>
        /// Gets or sets champion transform.
        /// </summary>
        [JsonProperty("championTransform")]
        public int ChampionTransform { get; set; }

        /// <summary>
        /// Gets or sets consumables purchased.
        /// </summary>
        [JsonProperty("consumablesPurshased")]
        public int ConsumablesPurchased { get; set; }

        /// <summary>
        /// Gets or sets Participant PUUID.
        /// </summary>
        [JsonProperty("puuid")]
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Riot ID Game Name.
        /// </summary>
        [JsonProperty("riotIdGameName")]
        public string RiotIdGameName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Riot ID Tag Line.
        /// </summary>
        [JsonProperty("riotIdTagline")]
        public string RiotIdTagLine { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Team ID.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }
    }
}
