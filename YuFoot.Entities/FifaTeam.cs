// <copyright file="FifaTeam.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Entities
{
    /// <summary>
    /// FifaTeam class.
    /// </summary>
    public class FifaTeam
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Code.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets Logo.
        /// </summary>
        public string? Logo { get; set; }
    }
}
