// <copyright file="GetPlatformByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Platforms.Queries.GetPlatformById
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// GetPlatformByIdQuery class.
    /// </summary>
    public class GetPlatformByIdQuery : IRequest<Platform?>
    {
        /// <summary>
        /// Gets or sets Platform ID.
        /// </summary>
        public int PlatformId { get; set; }
    }
}
