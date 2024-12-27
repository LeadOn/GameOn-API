// <copyright file="ObjectivesDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// ObjectivesDto class.
    /// </summary>
    public class ObjectivesDto
    {
        /// <summary>
        /// Gets or sets Baron objective.
        /// </summary>
        [JsonProperty("baron")]
        public ObjectiveDto Baron { get; set; } = new ObjectiveDto();

        /// <summary>
        /// Gets or sets champion objective.
        /// </summary>
        [JsonProperty("champion")]
        public ObjectiveDto Champion { get; set; } = new ObjectiveDto();

        /// <summary>
        /// Gets or sets dragon objective.
        /// </summary>
        [JsonProperty("dragon")]
        public ObjectiveDto Dragon { get; set; } = new ObjectiveDto();

        /// <summary>
        /// Gets or sets horde objective.
        /// </summary>
        [JsonProperty("horde")]
        public ObjectiveDto Horde { get; set; } = new ObjectiveDto();

        /// <summary>
        /// Gets or sets inhibitor objective.
        /// </summary>
        [JsonProperty("inhibitor")]
        public ObjectiveDto Inhibitor { get; set; } = new ObjectiveDto();

        /// <summary>
        /// Gets or sets Rift Herald objective.
        /// </summary>
        [JsonProperty("riftHerald")]
        public ObjectiveDto RiftHerald { get; set; } = new ObjectiveDto();

        /// <summary>
        /// Gets or sets Tower objective.
        /// </summary>
        [JsonProperty("tower")]
        public ObjectiveDto Tower { get; set; } = new ObjectiveDto();
    }
}
