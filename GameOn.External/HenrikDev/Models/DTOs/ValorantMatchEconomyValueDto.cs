// <copyright file="ValorantMatchEconomyValueDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Overall and per-round average credit value inside a HenrikDev v4 match player economy object.
    /// </summary>
    public class ValorantMatchEconomyValueDto
    {
        /// <summary>
        /// Gets or sets the total credits across all rounds.
        /// </summary>
        [JsonProperty("overall")]
        public int? Overall { get; set; }

        /// <summary>
        /// Gets or sets the average credits per round.
        /// </summary>
        [JsonProperty("average")]
        public double? Average { get; set; }
    }
}
