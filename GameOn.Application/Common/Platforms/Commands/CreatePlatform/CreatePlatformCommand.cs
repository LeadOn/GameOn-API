// <copyright file="CreatePlatformCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Platforms.Commands.CreatePlatform
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// CreatePlatformCommand class.
    /// </summary>
    public class CreatePlatformCommand : IRequest<Platform>
    {
        /// <summary>
        /// Gets or sets platform name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
