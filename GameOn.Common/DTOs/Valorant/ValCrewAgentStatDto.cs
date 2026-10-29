// <copyright file="ValCrewAgentStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// An agent's numbers across the whole crew (see <see cref="ValGlobalStatsDto.TopAgents"/>), plus who
    /// played it the most. A subclass for the same reason as <c>LoLCrewChampionStatDto</c>: on a single
    /// account's breakdown the top player would always be that account.
    /// </summary>
    public class ValCrewAgentStatDto : ValAgentStatDto
    {
        /// <summary>
        /// Gets or sets the account with the most games on this agent, within the same games as the rest of the
        /// entry. Ties are broken by wins, then by the lowest player ID. Never null.
        /// </summary>
        public ValTopPlayerDto TopPlayer { get; set; } = null!;
    }
}
