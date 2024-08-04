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
        /// Gets or sets Username.
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = "Default user";

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "Default password";
    }
}
