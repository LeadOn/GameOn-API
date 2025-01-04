// <copyright file="GetAllPlatformsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Platforms.Queries.GetAllPlatforms
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllPlatformsQuery class.
    /// </summary>
    public class GetAllPlatformsQuery : IRequest<IEnumerable<Platform>>
    {
    }
}
