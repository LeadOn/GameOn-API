// <copyright file="CreateHighlightCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Highlights.Commands.CreateHighlight
{
    using MediatR;
    using YuGames.Domain;

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
