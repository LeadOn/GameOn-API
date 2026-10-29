// <copyright file="ValTopPlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// The account that played an agent the most, within the same games as the <see cref="ValCrewAgentStatDto"/>
    /// holding it.
    /// </summary>
    public class ValTopPlayerDto
    {
        /// <summary>
        /// Gets or sets the account. Identity fields only. Per account: with smurfs included, a member's main
        /// and smurf compete separately.
        /// </summary>
        public PlayerDto Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets how many of the agent's games this account played. Always at least 1.
        /// </summary>
        public int GamesPlayed { get; set; }
    }
}
