// <copyright file="UpdateValorantPlayerAdminResultDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    using GameOn.Common.DTOs.Common;

    /// <summary>
    /// Result of an admin update of a player's Riot ID followed by a Valorant refresh.
    /// </summary>
    public class UpdateValorantPlayerAdminResultDto
    {
        /// <summary>
        /// Gets or sets the outcome of the update.
        /// </summary>
        public RiotIdUpdateStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the refreshed player, when the update succeeded.
        /// </summary>
        public PlayerDto? Player { get; set; }
    }
}
