// <copyright file="LoLQueue.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLQueue class.
    /// </summary>
    public class LoLQueue
    {
        /// <summary>
        /// Gets or sets Queue ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Map name.
        /// </summary>
        public string Map { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets Notes.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets League of Legends Games.
        /// </summary>
        [JsonIgnore]
        public virtual List<LoLGame> LeagueOfLegendsGames { get; set; } = null!;
    }
}
