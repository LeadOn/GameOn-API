// <copyright file="GetChangelogByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Queries.GetChangelogById
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetChangelogByIdQuery class.
    /// </summary>
    public class GetChangelogByIdQuery : IRequest<Changelog?>
    {
        /// <summary>
        /// Gets or sets Changelog ID.
        /// </summary>
        public int Id { get; set; }
    }
}
