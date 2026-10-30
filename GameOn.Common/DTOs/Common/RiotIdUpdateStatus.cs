// <copyright file="RiotIdUpdateStatus.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Common
{
    /// <summary>
    /// Outcome of a profile update that may change the player's Riot ID. A refusal is named rather than
    /// answered with the unchanged account, which a client cannot tell apart from a successful save.
    /// </summary>
    public enum RiotIdUpdateStatus
    {
        /// <summary>
        /// Profile updated.
        /// </summary>
        Updated,

        /// <summary>
        /// The requested Riot ID already belongs to another player. Nothing was saved.
        /// </summary>
        RiotIdTaken,
    }
}
