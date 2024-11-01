// <copyright file="GetAllChangelogQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Changelogs.Queries.GetAllChangelogs
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllChangelogQuery class.
    /// </summary>
    public class GetAllChangelogQuery : IRequest<IEnumerable<Changelog>>
    {
    }
}
