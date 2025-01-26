// <copyright file="GetGameTimelineByMatchIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetGameTimelineByMatchId
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetGameTimelineByMatchIdQuery class.
    /// </summary>
    public class GetGameTimelineByMatchIdQuery : IRequest<List<LoLGameTimelineFrame>>
    {
        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;
    }
}
