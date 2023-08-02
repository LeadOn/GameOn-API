﻿// <copyright file="GamePlayed.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Entities
{
    /// <summary>
    /// GamePlayed class.
    /// </summary>
    public class GamePlayed
    {
        /// <summary>
        /// Gets or sets player's ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets game date.
        /// </summary>
        public DateTime PlayedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Team Code 1
        /// </summary>
        public string? TeamCode1 { get; set; }

        /// <summary>
        /// Gets or sets Team Code 2.
        /// </summary>
        public string? TeamCode2 { get; set; }

        /// <summary>
        /// Gets or sets team 1 score.
        /// </summary>
        public int TeamScore1 { get; set; }

        /// <summary>
        /// Gets or sets team 2 score.
        /// </summary>
        public int TeamScore2 { get; set; }
    }
}