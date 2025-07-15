// <copyright file="UpdatePlayerProfilePictureCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Commands.UpdatePlayerProfilePicture
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// UpdatePlayerProfilePictureCommand class.
    /// </summary>
    public class UpdatePlayerProfilePictureCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets File.
        /// </summary>
        public IFormFile File { get; set; }
    }
}
