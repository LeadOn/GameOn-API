// <copyright file="GetFifaGamePlayedByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaGamePlayed.Queries.GetFifaGamePlayedById
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetFifaGamePlayedByIdQuery class.
    /// </summary>
    public class GetFifaGamePlayedByIdQuery : IRequest<FifaGamePlayedDto?>
    {
        /// <summary>
        /// Gets or sets Game ID.
        /// </summary>
        public int FifaGamePlayedId { get; set; }
    }
}
