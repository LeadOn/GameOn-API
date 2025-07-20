// <copyright file="GetProfilePictureQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Queries.GetProfilePicture
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetProfilePictureQuery class.
    /// </summary>
    public class GetProfilePictureQuery : IRequest<ProfilePictureDto>
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
