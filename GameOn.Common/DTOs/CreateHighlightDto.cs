// <copyright file="CreateHighlightDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    /// <summary>
    /// CreateHighlightDto class.
    /// </summary>
    public class CreateHighlightDto
    {
        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; } = "INVALID NAME";

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets FIFA Game ID.
        /// </summary>
        public int FifaGameId { get; set; }

        /// <summary>
        /// Gets or sets External URL.
        /// </summary>
        public string? ExternalUrl { get; set; }
    }
}
