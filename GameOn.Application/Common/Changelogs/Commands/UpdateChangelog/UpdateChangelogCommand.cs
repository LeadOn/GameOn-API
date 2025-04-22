// <copyright file="UpdateChangelogCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Changelogs.Commands.UpdateChangelog
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;
    
    /// <summary>
    /// UpdateChangelogCommand class.
    /// </summary>
    public class UpdateChangelogCommand : IRequest<Changelog>
    {
        /// <summary>
        /// Gets or sets Changelog to update.
        /// </summary>
        public Changelog Changelog { get; set; } = new Changelog();
    }
}
