// <copyright file="Highlight.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Entities
{
    /// <summary>
    /// Highlight class.
    /// </summary>
    public class Highlight
    {
        /// <summary>
        /// Gets or sets Highlight ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Highlight name.
        /// </summary>
        public string Name { get; set; } = "Random name";

        /// <summary>
        /// Gets or sets Created By ID.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Gets or sets Game ID.
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Gets or sets External URL.
        /// </summary>
        public string ExternalUrl { get; set; } = "https://www.valentinvirot.fr/";

        /// <summary>
        /// Gets or sets Created by.
        /// </summary>
        public virtual Player CreatedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets Game.
        /// </summary>
        public virtual GamePlayed Game { get; set; } = null!;
    }
}