// <copyright file="LoLGame.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    /// <summary>
    /// LoLGame class.
    /// </summary>
    public class LoLGame
    {
        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        public long GameId { get; set; }

        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets End of Game Result.
        /// </summary>
        public string EndOfGameResult { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets game version.
        /// </summary>
        public string GameVersion { get; set; } = string.Empty;
    }
}