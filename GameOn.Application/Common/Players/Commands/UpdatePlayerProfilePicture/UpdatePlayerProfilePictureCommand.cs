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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public IFormFile File { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
