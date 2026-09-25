// <copyright file="GetLoLHomeStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Home.Queries.GetLoLHomeStats
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// GetLoLHomeStatsQuery class. Aggregated recap powering the League of Legends v2 home page.
    /// </summary>
    public class GetLoLHomeStatsQuery : IRequest<LoLHomeStatsDto>
    {
        /// <summary>
        /// Gets or sets the window of the "this week" blocks (weekly activity and fact of the week).
        /// Defaults to <see cref="LoLHomeWindow.CalendarWeek"/>, the historical behavior. The crew records
        /// ignore it and always cover the rolling month.
        /// </summary>
        public LoLHomeWindow Window { get; set; } = LoLHomeWindow.CalendarWeek;

        /// <summary>
        /// Gets or sets a value indicating whether smurf accounts count towards the recap. Defaults to
        /// true: a smurf's games were played, so they weigh on the week like any other. Set to false to
        /// read the page as one entry per member instead. Applies to every block, the crew records
        /// included, so the whole page keeps describing the same roster.
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether accounts outside the crew count towards the recap.
        /// Defaults to false: this page is the crew's dashboard, and an account left out of the crew is
        /// precisely one whose numbers stopped counting. Applies to every block, the crew records
        /// included.
        /// </summary>
        public bool IncludeOutOfCrew { get; set; }
    }
}
