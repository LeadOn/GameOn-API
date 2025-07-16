// <copyright file="ProfilePictureDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Domain;

    /// <summary>
    /// ProfilePictureDto class.
    /// </summary>
    public class ProfilePictureDto
    {
        /// <summary>
        /// Gets or sets File Stream.
        /// </summary>
        public Stream? FileStream { get; set; }

        /// <summary>
        /// Gets or sets File name.
        /// </summary>
        public string FileName { get; set; } = string.Empty;
    }
}