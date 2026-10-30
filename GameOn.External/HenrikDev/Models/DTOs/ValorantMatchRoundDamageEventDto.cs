// <copyright file="ValorantMatchRoundDamageEventDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Damage dealt to one player during a round, inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchRoundDamageEventDto
    {
        /// <summary>
        /// Gets or sets the player who received the damage.
        /// </summary>
        [JsonProperty("player")]
        public ValorantMatchPlayerRefDto? Player { get; set; }

        /// <summary>
        /// Gets or sets the damage dealt.
        /// </summary>
        [JsonProperty("damage")]
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the number of head hits.
        /// </summary>
        [JsonProperty("headshots")]
        public int Headshots { get; set; }

        /// <summary>
        /// Gets or sets the number of body hits.
        /// </summary>
        [JsonProperty("bodyshots")]
        public int Bodyshots { get; set; }

        /// <summary>
        /// Gets or sets the number of leg hits.
        /// </summary>
        [JsonProperty("legshots")]
        public int Legshots { get; set; }
    }
}
