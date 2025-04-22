// <copyright file="Changelog.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    /// <summary>
    /// Changelog class.
    /// </summary>
    public class Changelog
    {
        /// <summary>
        /// Gets or sets Changelog ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets publication date.
        /// </summary>
        public DateTime PublicationDate { get; set; }

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