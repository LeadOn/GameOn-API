// <copyright file="GetLatestChangelog.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Queries.GetLatestChangelog
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetLatestChangelogQuery class.
    /// </summary>
    public class GetLatestChangelogQuery : IRequest<Changelog>
    {
    }
}
