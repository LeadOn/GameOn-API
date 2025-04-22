// <copyright file="CreateChangelogDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Domain;

    /// <summary>
    /// CreateChangelogDto class.
    /// </summary>
    public class CreateChangelogDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateChangelogDto"/> class.
        /// </summary>
        public CreateChangelogDto()
        {
        }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets Type.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether changelog is published or not.
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets version.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets context.
        /// </summary>
        public string? Context { get; set; }

        /// <summary>
        /// Gets or sets new features.
        /// </summary>
        public List<string>? NewFeatures { get; set; }

        /// <summary>
        /// Gets or sets updated features.
        /// </summary>
        public List<string>? UpdatedFeatures { get; set; }

        /// <summary>
        /// Gets or sets removed features.
        /// </summary>
        public List<string>? RemovedFeatures { get; set; }
    }
}