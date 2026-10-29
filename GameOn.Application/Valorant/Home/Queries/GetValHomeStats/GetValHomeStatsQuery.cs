// <copyright file="GetValHomeStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Home.Queries.GetValHomeStats
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.Valorant;
    using MediatR;

    /// <summary>
    /// GetValHomeStatsQuery class. Aggregated recap powering the Valorant home page.
    /// </summary>
    public class GetValHomeStatsQuery : IRequest<ValHomeStatsDto>
    {
        /// <summary>
        /// Gets or sets the window of the "this week" blocks (weekly activity and fact of the week). The crew
        /// records ignore it and always cover the rolling month.
        /// </summary>
        public LoLHomeWindow Window { get; set; } = LoLHomeWindow.CalendarWeek;

        /// <summary>
        /// Gets or sets a value indicating whether smurf accounts count towards the recap. Defaults to true.
        /// Applies to every block, the crew records included, so the whole page describes the same roster.
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether accounts outside the crew count towards the recap. Defaults
        /// to false. Applies to every block, the crew records included.
        /// </summary>
        public bool IncludeOutOfCrew { get; set; }
    }
}
