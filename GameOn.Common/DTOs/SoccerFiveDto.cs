// <copyright file="SoccerFiveDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Domain;

    /// <summary>
    /// SoccerFiveDto class.
    /// </summary>
    public class SoccerFiveDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoccerFiveDto"/> class.
        /// </summary>
        public SoccerFiveDto()
        {
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets PlannedOn.
        /// </summary>
        public DateTime? PlannedOn { get; set; }
    }
}