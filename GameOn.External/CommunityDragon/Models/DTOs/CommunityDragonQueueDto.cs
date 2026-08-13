// <copyright file="CommunityDragonQueueDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.CommunityDragon.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// CommunityDragonQueueDto class. Mirrors the queue objects served by Community Dragon's
    /// game data mirror (more exhaustive, but unofficial and untranslated field names, unlike Riot's queues.json).
    /// </summary>
    public class CommunityDragonQueueDto
    {
        /// <summary>
        /// Gets or sets Queue ID. Same referential as Riot's queueId.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Queue name.
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets Queue short name.
        /// </summary>
        [JsonProperty("shortName")]
        public string? ShortName { get; set; }

        /// <summary>
        /// Gets or sets Queue description.
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets Queue detailed description.
        /// </summary>
        [JsonProperty("detailedDescription")]
        public string? DetailedDescription { get; set; }
    }
}
