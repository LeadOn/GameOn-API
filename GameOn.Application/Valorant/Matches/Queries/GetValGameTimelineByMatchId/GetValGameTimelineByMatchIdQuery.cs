// <copyright file="GetValGameTimelineByMatchIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Queries.GetValGameTimelineByMatchId
{
    using GameOn.Common.DTOs.Valorant;
    using MediatR;

    /// <summary>
    /// Retrieves the round-by-round breakdown and chronological kill events for a Valorant match.
    /// </summary>
    public class GetValGameTimelineByMatchIdQuery : IRequest<ValGameTimelineDto?>
    {
        /// <summary>
        /// Gets or sets the Valorant match UUID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;
    }
}
