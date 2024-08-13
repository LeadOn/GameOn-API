// <copyright file="UpdateSoccerFiveDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    /// <summary>
    /// UpdateSoccerFiveDto class.
    /// </summary>
    public class UpdateSoccerFiveDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// Gets or sets planned on.
        /// </summary>
        public DateTime? PlannedOn { get; set; }
    }
}