// <copyright file="ValBackgroundTaskKind.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Kind of Valorant background task. Every kind reads HenrikDev at low priority, so none of them ever takes
    /// the requests the regular refresh and the players need.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ValBackgroundTaskKind
    {
        /// <summary>
        /// Imports a player's history stored by HenrikDev: the MMR history, then every stored match missing
        /// from the database (1 HenrikDev call per match).
        /// </summary>
        Backfill,

        /// <summary>
        /// Re-imports matches already in database, to fill the columns added since they were imported
        /// (1 HenrikDev call per match).
        /// </summary>
        Reimport,

        /// <summary>
        /// Refreshes a player's accolades, agent mastery and Premier team (3 HenrikDev calls).
        /// </summary>
        Extras,
    }
}
