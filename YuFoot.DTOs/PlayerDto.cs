// <copyright file="PlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.DTOs
{
    /// <summary>
    /// Player DTO class.
    /// </summary>
    public class PlayerDto
    {
        /// <summary>
        /// Gets or sets player's ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets player's full name.
        /// </summary>
        public string FullName { get; set; } = "John Doe";

        /// <summary>
        /// Gets or sets player's nickname.
        /// </summary>
        public string? Nickname { get; set; } = "J0hnD03";

        /// <summary>
        /// Gets or sets player's profile picture URL.
        /// </summary>
        public string? ProfilePictureUrl { get; set; }

        /// <summary>
        /// Gets or sets player's creation date.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets number of match played.
        /// </summary>
        public int MatchPlayed { get; set; }

        /// <summary>
        /// Gets or sets number of wins.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets number of losses.
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets number of draws.
        /// </summary>
        public int Draws { get; set; }
    }
}
