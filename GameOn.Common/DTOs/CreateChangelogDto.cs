// <copyright file="CreateSoccerFiveDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Domain;

    /// <summary>
    /// CreateSoccerFiveDto class.
    /// </summary>
    public class CreateSoccerFiveDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSoccerFiveDto"/> class.
        /// </summary>
        public CreateSoccerFiveDto()
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