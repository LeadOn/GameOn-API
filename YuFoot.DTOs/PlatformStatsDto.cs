﻿// <copyright file="PlatformStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.DTOs
{
    using YuFoot.Entities;

    /// <summary>
    /// Platform Stats DTO class.
    /// </summary>
    public class PlatformStatsDto
    {
        /// <summary>
        /// Gets or sets Platform.
        /// </summary>
        public Platform Platform { get; set; } = new Platform();

        /// <summary>
        /// Gets or sets wins.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets losses.
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets draws.
        /// </summary>
        public int Draws { get; set; }

        /// <summary>
        /// Gets or sets Average Goal Given.
        /// </summary>
        public float AverageGoalGiven { get; set; }

        /// <summary>
        /// Gets or sets Average Goal Given.
        /// </summary>
        public float AverageGoalTaken { get; set; }

        /// <summary>
        /// Gets or sets Goals given.
        /// </summary>
        public int GoalsGiven { get; set; }

        /// <summary>
        /// Gets or sets Goals taken.
        /// </summary>
        public int GoalsTaken { get; set; }

        /// <summary>
        /// Gets or sets Goal difference.
        /// </summary>
        public int GoalDifference { get; set; }
    }
}
