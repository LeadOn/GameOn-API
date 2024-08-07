// <copyright file="RegisterDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    /// <summary>
    /// RegisterDto class.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Nickname.
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
