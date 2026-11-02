// <copyright file="ValHeatmapType.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// What a Valorant heatmap shows.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ValHeatmapType
    {
        /// <summary>
        /// Where the player stood when they got a kill.
        /// </summary>
        Kills,

        /// <summary>
        /// Where the player died.
        /// </summary>
        Deaths,
    }
}
