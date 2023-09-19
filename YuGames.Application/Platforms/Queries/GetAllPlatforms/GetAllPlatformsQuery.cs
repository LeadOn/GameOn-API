// <copyright file="GetAllPlatformsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Platforms.Queries.GetAllPlatforms
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// GetAllPlatformsQuery class.
    /// </summary>
    public class GetAllPlatformsQuery : IRequest<IEnumerable<Platform>>
    {
    }
}
