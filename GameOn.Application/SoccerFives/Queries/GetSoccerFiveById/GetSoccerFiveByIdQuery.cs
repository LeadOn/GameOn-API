// <copyright file="GetSoccerFiveByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.TournamentPlayers.Queries.GetSoccerFiveById
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetSoccerFiveByIdQuery class.
    /// </summary>
    public class GetSoccerFiveByIdQuery : IRequest<SoccerFiveDto?>
    {
        /// <summary>
        /// Gets or sets Soccer Five ID.
        /// </summary>
        public int SoccerFiveId { get; set; }
    }
}
