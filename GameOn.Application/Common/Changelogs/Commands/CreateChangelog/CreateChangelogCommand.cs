// <copyright file="CreateChangelogCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Commands.CreateChangelog
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// CreateChangelogCommand class.
    /// </summary>
    public class CreateChangelogCommand : IRequest<Changelog>
    {
        /// <summary>
        /// Gets or sets Changelog.
        /// </summary>
        public Changelog Changelog { get; set; } = new Changelog();
    }
}
