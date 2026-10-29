// <copyright file="GetValGlobalStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Stats.Queries.GetValGlobalStats
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.Valorant;
    using MediatR;

    /// <summary>
    /// GetValGlobalStatsQuery class. Crew-wide Valorant fun stats.
    /// </summary>
    public class GetValGlobalStatsQuery : IRequest<ValGlobalStatsDto>
    {
        /// <summary>
        /// Gets or sets a value indicating whether only competitive games are included.
        /// </summary>
        public bool RankedOnly { get; set; }

        /// <summary>
        /// Gets or sets the rolling time window restriction.
        /// </summary>
        public LoLStatsPeriod Period { get; set; } = LoLStatsPeriod.AllTime;

        /// <summary>
        /// Gets or sets a value indicating whether smurf accounts take part in the records. Defaults to true,
        /// like <c>GET lol/Stats/global</c>: an award is about a game that was played, and a smurf's games were
        /// played. Set to false to drop the secondary accounts entirely (never merged into their owner).
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether accounts outside the crew take part in the records. Defaults
        /// to false: these records describe the crew.
        /// </summary>
        public bool IncludeOutOfCrew { get; set; }
    }
}
