// <copyright file="UpdatePlatformCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Platforms.Commands.UpdatePlatform
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// UpdatePlatformCommand class.
    /// </summary>
    public class UpdatePlatformCommand : IRequest<Platform>
    {
        /// <summary>
        /// Gets or sets platform ID.
        /// </summary>
        public int PlatformId { get; set; }

        /// <summary>
        /// Gets or sets platform name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
