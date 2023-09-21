// <copyright file="GetFifaGamePlayedByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Queries.GetFifaGamePlayedById
{
    using MediatR;
    using YuGames.Common.DTOs;

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
