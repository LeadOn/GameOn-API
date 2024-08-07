// <copyright file="UserLoginDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// UserLoginDto class.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; } = "test@exemple.com";

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; } = "Default password";
    }
}
