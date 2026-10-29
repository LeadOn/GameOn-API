// <copyright file="ValAgentStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// How much a Valorant agent was played over a period, and how well. Rates are computed on totals, like
    /// <see cref="ValorantPlayerPerformanceStatsDto"/>.
    /// </summary>
    public class ValAgentStatDto
    {
        /// <summary>
        /// Gets or sets the agent UUID.
        /// </summary>
        public string? AgentId { get; set; }

        /// <summary>
        /// Gets or sets the agent display name (e.g. "Jett", "KAY/O").
        /// </summary>
        public string AgentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the square agent icon URL on <c>media.valorant-api.com</c>. Null when the agent UUID is unknown.
        /// </summary>
        public string? AgentIconUrl { get; set; }

        /// <summary>
        /// Gets or sets the number of games played on this agent.
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets the number of games won on this agent.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the win rate on this agent, as a percentage.
        /// </summary>
        public double WinRate { get; set; }

        /// <summary>
        /// Gets or sets the KDA ratio on this agent, on totals.
        /// </summary>
        public double Kda { get; set; }

        /// <summary>
        /// Gets or sets the Average Combat Score on this agent. 0 when no game has derived stats.
        /// </summary>
        public double Acs { get; set; }

        /// <summary>
        /// Gets or sets the headshot percentage on this agent. 0 when no shot hit.
        /// </summary>
        public double HeadshotPercent { get; set; }
    }
}
