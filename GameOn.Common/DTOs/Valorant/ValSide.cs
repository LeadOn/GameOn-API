// <copyright file="ValSide.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Side of a Valorant round.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ValSide
    {
        /// <summary>
        /// Attackers (carry the spike).
        /// </summary>
        Attack,

        /// <summary>
        /// Defenders.
        /// </summary>
        Defense,
    }
}
