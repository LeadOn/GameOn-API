// <copyright file="ValorantMatchRoundPlayerScoreDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Player's combat numbers for one round, inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchRoundPlayerScoreDto
    {
        /// <summary>
        /// Gets or sets the combat score earned in the round (can be negative).
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the number of kills in the round.
        /// </summary>
        [JsonProperty("kills")]
        public int Kills { get; set; }

        /// <summary>
        /// Gets or sets the number of head hits in the round.
        /// </summary>
        [JsonProperty("headshots")]
        public int Headshots { get; set; }

        /// <summary>
        /// Gets or sets the number of body hits in the round.
        /// </summary>
        [JsonProperty("bodyshots")]
        public int Bodyshots { get; set; }

        /// <summary>
        /// Gets or sets the number of leg hits in the round.
        /// </summary>
        [JsonProperty("legshots")]
        public int Legshots { get; set; }
    }
}
