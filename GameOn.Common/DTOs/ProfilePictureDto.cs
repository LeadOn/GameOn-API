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
        public Stream? FileStream { get; set; }

        public string FileName { get; set; }
    }
}