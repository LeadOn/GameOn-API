// <copyright file="ValorantMatchEconomyDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Economy summary (spent credits and loadout value) inside a HenrikDev v4 match player entry.
    /// </summary>
    public class ValorantMatchEconomyDto
    {
        /// <summary>
        /// Gets or sets the spent credit totals and averages.
        /// </summary>
        [JsonProperty("spent")]
        public ValorantMatchEconomyValueDto? Spent { get; set; }

        /// <summary>
        /// Gets or sets the loadout credit totals and averages.
        /// </summary>
        [JsonProperty("loadout_value")]
        public ValorantMatchEconomyValueDto? LoadoutValue { get; set; }
    }
}
