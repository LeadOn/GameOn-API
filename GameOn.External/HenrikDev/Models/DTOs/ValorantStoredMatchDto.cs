// <copyright file="ValorantStoredMatchDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Entry of HenrikDev's stored match history (stored-matches v1). Only the metadata is read: the match itself is fetched in full from match v4.
    /// </summary>
    public class ValorantStoredMatchDto
    {
        /// <summary>
        /// Gets or sets the match metadata.
        /// </summary>
        [JsonProperty("meta")]
        public ValorantStoredMatchMetaDto? Meta { get; set; }
    }
}
