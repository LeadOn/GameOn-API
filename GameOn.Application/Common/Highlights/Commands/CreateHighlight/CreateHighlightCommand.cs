// <copyright file="CreateHighlightCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Highlights.Commands.CreateHighlight
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// CreateHighlightCommand class.
    /// </summary>
    public class CreateHighlightCommand : IRequest<Highlight>
    {
        /// <summary>
        /// Gets or sets Highlight.
        /// </summary>
        public Highlight Highlight { get; set; } = new Highlight();
    }
}
