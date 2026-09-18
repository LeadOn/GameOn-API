// <copyright file="GetLoLGlobalStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Stats.Queries.GetLoLGlobalStats
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// GetLoLGlobalStatsQuery class.
    /// </summary>
    public class GetLoLGlobalStatsQuery : IRequest<LoLGlobalStatsDto>
    {
        /// <summary>
        /// Gets or sets a value indicating whether only ranked games (Solo/Duo and Flex) are included.
        /// </summary>
        public bool RankedOnly { get; set; }

        /// <summary>
        /// Gets or sets the queue restriction. Solo or Flex implies ranked games only.
        /// </summary>
        public LoLQueueFilter Queue { get; set; } = LoLQueueFilter.All;

        /// <summary>
        /// Gets or sets the rolling time window restriction.
        /// </summary>
        public LoLStatsPeriod Period { get; set; } = LoLStatsPeriod.AllTime;

        /// <summary>
        /// Gets or sets a value indicating whether smurf accounts take part in the records. Defaults to
        /// true, like every other player-facing read: an award is about a game that was played, and a
        /// smurf's games were played. Set to false to read the records as one entry per member instead.
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether accounts outside the crew take part in the records.
        /// Defaults to false, like <c>GetAllLeaguePlayersQuery</c>: these records describe the crew, and
        /// an account left out of it is precisely one whose numbers stopped counting.
        /// </summary>
        public bool IncludeOutOfCrew { get; set; }
    }
}
