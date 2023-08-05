// <copyright file="UpdatePlayerModel.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.WebAPI.Models
{
    /// <summary>
    /// Update user model.
    /// </summary>
    public class UpdatePlayerModel
    {
        /// <summary>
        /// Gets or sets Full Name.
        /// </summary>
        public string FullName { get; set; } = "INVALID NICKNAME";

        /// <summary>
        /// Gets or sets Nickname.
        /// </summary>
        public string Nickname { get; set; } = "INVALID FULLNAME";

        /// <summary>
        /// Gets or sets Profile Picture URL.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = "INVALID URL";
    }
}
