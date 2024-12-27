// <copyright file="TeamDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// TeamDto class.
    /// </summary>
    public class TeamDto
    {
        /// <summary>
        /// Gets or sets bans.
        /// </summary>
        [JsonProperty("bans")]
        public List<BanDto> Bans { get; set; } = new List<BanDto>();

        /// <summary>
        /// Gets or sets objectives.
        /// </summary>
        [JsonProperty("objectives")]
        public ObjectivesDto Objectives { get; set; } = new ObjectivesDto();

        /// <summary>
        /// Gets or sets team ID.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether team as won or not.
        /// </summary>
        [JsonProperty("win")]
        public bool HasWon { get; set; }
    }
}
