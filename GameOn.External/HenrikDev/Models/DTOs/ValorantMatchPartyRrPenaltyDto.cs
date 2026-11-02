// <copyright file="ValorantMatchPartyRrPenaltyDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// RR penalty applied to a party in a match.
    /// </summary>
    public class ValorantMatchPartyRrPenaltyDto
    {
        /// <summary>
        /// Gets or sets the party UUID.
        /// </summary>
        [JsonProperty("party_id")]
        public string? PartyId { get; set; }

        /// <summary>
        /// Gets or sets the penalty factor (e.g. 0.25).
        /// </summary>
        [JsonProperty("penalty")]
        public double Penalty { get; set; }
    }
}
