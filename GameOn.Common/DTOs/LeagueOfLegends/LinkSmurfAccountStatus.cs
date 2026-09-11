// <copyright file="LinkSmurfAccountStatus.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// Outcome of a smurf account link attempt. Every refusal is named rather than collapsed into a null
    /// result, because the four of them map to different HTTP answers and the admin needs to know which
    /// one they hit.
    /// </summary>
    public enum LinkSmurfAccountStatus
    {
        /// <summary>
        /// Account linked to the primary player.
        /// </summary>
        Linked,

        /// <summary>
        /// The primary player does not exist.
        /// </summary>
        PrimaryNotFound,

        /// <summary>
        /// The target primary player is itself a smurf: chains are not allowed, the link has to point at
        /// the real account.
        /// </summary>
        PrimaryIsSmurf,

        /// <summary>
        /// Riot Games does not know that Riot ID.
        /// </summary>
        RiotAccountNotFound,

        /// <summary>
        /// That Riot account is already attached to another player, or is a registered player of its own
        /// (it can log in, or holds smurfs). Unlink it first.
        /// </summary>
        AccountNotAvailable,
    }
}
