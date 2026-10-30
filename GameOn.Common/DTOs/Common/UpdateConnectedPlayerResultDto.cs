// <copyright file="UpdateConnectedPlayerResultDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Common
{
    using GameOn.Domain;

    /// <summary>
    /// Result of an update of the connected player's profile.
    /// </summary>
    public class UpdateConnectedPlayerResultDto
    {
        /// <summary>
        /// Gets or sets the outcome of the update.
        /// </summary>
        public RiotIdUpdateStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the updated player, when the update succeeded.
        /// </summary>
        public Player? Player { get; set; }
    }
}
