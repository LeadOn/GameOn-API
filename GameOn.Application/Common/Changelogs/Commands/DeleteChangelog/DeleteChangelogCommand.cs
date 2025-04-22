// <copyright file="DeleteChangelogCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Commands.DeleteChangelog
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// DeleteChangelogCommand class.
    /// </summary>
    public class DeleteChangelogCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Changelog ID.
        /// </summary>
        public int Id { get; set; }
    }
}
